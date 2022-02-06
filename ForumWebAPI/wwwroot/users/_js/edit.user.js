import {
	setRolesRequest,
	getUserRequest,
	resetUserPasswordRequest,
	updateUserRequest,
} from "./user.api.js";

import {
	validatePassword,
	validatePasswordConfirmation,
} from "./user.validation.js";
import "/components/form.field/form.field.js";
import "/components/submit.button/submit.button.js";
import "/components/alert/alert.js";

const formProfile = document.querySelector("#userProfile-form");
const formChangePassword = document.querySelector("#changePassword-form");
const formRoles = document.querySelector("#userRoles-form");
const alert = document.querySelector("alert-message");
const userName = new URLSearchParams(location.search).get("userName");
const header = document.querySelector("auth-header");

loadUser();

function loadUser() {
	if (formProfile.loading) return;

	formProfile.loading = true;
	getUserRequest(userName)
		.then((user) => fillForms(user))
		.catch(() => {
			alert.showAlert("Не удалось загрузить профиль пользователя");
		})
		.finally(() => formProfile.loading = false);
}

function fillForms(user) {
	for (let key in user) {
		if (formProfile[key]) {
			formProfile[key].value = user[key];
		}
	}

	header.setAttribute("page-title", `Профиль ${user.userName}`);

	for (let role of user.roles) {
		const input = formRoles.querySelector(`input[value="${role}"]`);
		if (input) {
			input.checked = true;
		}
	}
	formProfile.addEventListener("submit", (evt) => {
		evt.preventDefault();
		submitProfile();
	});
	formChangePassword.addEventListener("submit", (evt) => {
		evt.preventDefault();
		submitPassword(user);
	});
	formRoles.addEventListener("submit", (evt) => {
		evt.preventDefault();
		submitRoles(user);
	});
}

function submitProfile() {
	if (formProfile.submitting) return;

	const submitButton = formProfile.querySelector('button[type="submit"]');
	formProfile.submitting = true;
	submitButton.submitting = true;
	alert.hideAlert();

	const user = {
		userName,
		email: formProfile["email"].value,
		firstName: formProfile["firstName"].value,
		lastName: formProfile["lastName"].value,
	};
	updateUserRequest(user)
		.then(() => {
			alert.showAlert("Пользователь успешно обновлен!", "success");
		})
		.catch((err) => {
			console.error(err);
			alert.showAlert("Не удалось обновить профиль пользователя");
		})
		.finally(() => {
			formProfile.submitting = false;
			submitButton.submitting = false;
		});
}

const passwordField = formChangePassword.querySelector(
	'form-field[name="password"]'
);
const passwordConfirmationField = formChangePassword.querySelector(
	'form-field[name="passwordConfirmation"]'
);

passwordField.validation = (password) => {
	return (
		validatePassword(password) ||
		(passwordConfirmationField.touched
			? passwordConfirmationField.validate()
			: undefined)
	);
};

passwordConfirmationField.validation = () => {
	return validatePasswordConfirmation(
		formChangePassword["password"].value,
		formChangePassword["passwordConfirmation"].value
	);
};

function submitPassword(user) {
	if (formChangePassword.submitting) return;

	const password = formChangePassword["password"].value;
	const passwordConfirmation =
		formChangePassword["passwordConfirmation"].value;
	const validationResult =
		validatePassword(password) ||
		validatePasswordConfirmation(password, passwordConfirmation);

	if (validationResult) {
		alert.showAlert(validationResult);
		return;
	}

	const submitButton = formProfile.querySelector('button[type="submit"]');
	formChangePassword.submitting = true;
	submitButton.submitting = true;
	alert.hideAlert();

	resetUserPasswordRequest(user, password)
		.then(() => {
			alert.showAlert("Пароль успешно обновлен!", "success");
		})
		.catch((err) => {
			console.error(err);
			alert.showAlert("Не удалось обновить пароль пользователя.");
		})
		.finally(() => {
			formChangePassword.submitting = false;
			submitButton.submitting = false;
		});
}

function submitRoles(user) {
	if (formRoles.submitting) return;

	const submitButton = formRoles.querySelector('button[type="submit"]');
	formRoles.submitting = true;
	submitButton.submitting = true;
	alert.hideAlert();

	const checkedRoles = Array.from(formRoles["role"].values())
		.filter((v) => v.checked)
		.map((v) => v.value);

	setRolesRequest(user, checkedRoles)
		.then(() => {
			alert.showAlert("Роли пользователя успешно установлены!", "success");
		})
		.catch((err) => {
			console.error(err);
			alert.showAlert("Не удалось установить роли пользователя.");
		})
		.finally(() => {
			formRoles.submitting = false;
			submitButton.submitting = false;
		});
}