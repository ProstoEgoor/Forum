import api, { HttpStatusError } from "/utils/api.js";
import "/components/form.field/form.field.js";
import "/components/submit.button/submit.button.js";
import "/components/alert/alert.js";

import {
	validatePassword,
	validatePasswordConfirmation,
	validatePasswordFull,
} from "./password.validation.js";

function createUserRequest(user) {
	return api.post("account", user);
}

const form = document.querySelector("#form-profile");
const alert = document.querySelector("alert-message");


form.addEventListener("submit", (evt) => {
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

	const user = {
		userName: form["userName"].value,
		email: form["email"].value,
		firstName: form["firstName"].value,
		lastName: form["lastName"].value,
		password: form["password"].value,
	};

	createUserRequest(user)
		.then(() => {
			alert.showAlert("Пользователь успешно создан!", "success");
			setTimeout(() => location.assign("/auth/login.html"), 500);
		})
		.catch((err) => {
			alert.showAlert(err instanceof HttpStatusError ? err.message : "Не удалось создать пользователя");
		})
		.finally(() => {
			form.submitting = false;
			submitButton.submitting = false;
		});
});