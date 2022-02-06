const PASSWORD_PATTERN =
	/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$/;
export function validatePassword(password) {
	if (!password.match(PASSWORD_PATTERN)) {
		return (
			"Пароль должен содержать как минимум 8 символов, " +
			"одну строчную и одну заглавную латинскую букву, одну цифру и один специальный символ"
		);
	}
}
export function validatePasswordConfirmation(password, passwordConfirmation) {
	if (password != passwordConfirmation) {
		return "Пароль и подтверждение пароля не совпадают";
	}
}

export function validatePasswordFull(form) {
	return (
		validatePassword(form["password"].value) ||
		validatePasswordConfirmation(
			form["password"].value,
			form["passwordConfirmation"].value
		)
	);
}