import api from "/utils/api.js";
import "/components/form.field/form.field.js";
import "/components/submit.button/submit.button.js";
import "/components/alert/alert.js";

import {
	validatePassword,
	validatePasswordConfirmation,
	validatePasswordFull,
} from "./password.validation.js";

function getProfileRequest() {
	return api.get("account");
}

function updateProfileRequest(newProfile) {
	return api.put("account", newProfile);
}

const header = document.querySelector("auth-header");
const profileForm = document.getElementById("userProfile-form");
const alert = document.querySelector("alert-message");

profileForm.loading

header.addEventListener("loadProfile", (e) => {
	if (e.detail) {
		loadProfile(e.detail);
	}
})


function loadProfile(profile) {
	console.log(profile);
	profileForm.userName = profile.userName;
	profileForm.roles = profile.roles;
	profileForm["firstName"].value = profile.firstName;
	profileForm["lastName"].value = profile.lastName;
	profileForm["email"].value = profile.email;
	profileForm.onsubmit = submitForm; // заменит обработчик, если он уже был назначен

	profileForm.loading = false;
}

function submitForm(evt) {
	evt.preventDefault();
	const form = evt.target;
	const submitButton = form.querySelector('button[type="submit"]');

	if (form.submitting) return;

	form.submitting = true;
	submitButton.submitting = true;
	alert.hideAlert();

	updateProfileRequest({
		username: form.userName,
		roles: form.roles,
		firstName: form["firstName"].value,
		lastName: form["lastName"].value,
		email: form["email"].value,
	})
		.then(() => alert.showAlert("Профиль успешно обновлен!", "success"))
		.catch(() => alert.showAlert("Не удалось обновить данные профиля"))
		.finally(() => {
			form.submitting = false;
			submitButton.submitting = false;
		});
}

function changePasswordRequest(newPassword) {
	return api.put("account/password", { newPassword });
}

const passwordForm = document.getElementById("changePassword-form");
const passwordField = passwordForm.querySelector('form-field[name="password"]');
const passwordConfirmationField = passwordForm.querySelector(
	'form-field[name="passwordConfirmation"]'
);

passwordForm.addEventListener("submit", (evt) => {
	evt.preventDefault();
	const form = evt.target;
	const submitButton = form.querySelector('button[type="submit"]');

	if (form.submitting) return;

	const validationResult = validatePasswordFull(form);
	if (validationResult) {
		alert.showAlert(validationResult);
		return;
	}

	form.submitting = true;
	submitButton.submitting = true;
	alert.hideAlert();

	changePasswordRequest(form["password"].value)
		.then(() => {
			alert.showAlert("Пароль успешно изменен!", "success");
			form.reset();
		})
		.catch(() => alert.showAlert("Не удалось изменить пароль"))
		.finally(() => {
			form.submitting = false;
			submitButton.submitting = false;
		});
});

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
		passwordForm["password"].value,
		passwordForm["passwordConfirmation"].value
	);
};