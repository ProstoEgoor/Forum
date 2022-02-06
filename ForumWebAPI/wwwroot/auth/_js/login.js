import api, { HttpStatusError } from "/utils/api.js";
import "/components/submit.button/submit.button.js";
import "/components/alert/alert.js";

function loginRequest(userName, password, rememberMe) {
	return api.post("account/login", {
		userName,
		password,
		rememberMe,
	});
}

function goBack() {
	const fromUrl = new URLSearchParams(location.search).get("from");
	location.assign(fromUrl || "/");
}

const loginForm = document.getElementById("login-form");
const alert = document.querySelector("alert-message");

loginForm.addEventListener("submit", async (evt) => {
	evt.preventDefault();
	const form = evt.target;
	const submitButton = form.querySelector('button[type="submit"]');

	if (form.submitting) return;

	form.submitting = true;
	submitButton.submitting = true;
	alert.hideAlert();

	const userName = form["userName"].value;
	const password = form["password"].value;
	const rememberMe = form["rememberMe"].checked;

	try {
		await loginRequest(userName, password, rememberMe);
		goBack();
	} catch (err) {
		if (err instanceof HttpStatusError && err.status == 401) {
			alert.showAlert("Не удалось выполнить вход. Проверьте правильность ввода логина и пароля.");
		} else {
			alert.showAlert("Произошла неизвестная ошибка при попытке входа. Попробуйте позже или обратитесь к администратору.");
		}
	} finally {
		form.submitting = false;
		submitButton.submitting = false;
	}
});