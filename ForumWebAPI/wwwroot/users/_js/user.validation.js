import {
	validatePassword,
	validatePasswordConfirmation,
} from "/profile/_js/password.validation.js";
export {
	validatePassword,
	validatePasswordConfirmation,
} from "/profile/_js/password.validation.js";

export function validateCreation(form) {
	return (
		validatePassword(form["password"].value) ||
		validatePasswordConfirmation(
			form["password"].value,
			form["passwordConfirmation"].value
		)
	);
}