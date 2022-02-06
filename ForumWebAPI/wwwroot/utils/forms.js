export function attributeValueToBool(value) {
	return value === "" || (!!value && value !== "false");
}

export function attributeValueToFunction(value) {
	if (value instanceof Function) {
		return value;
	}
	const evaluation = eval(value);
	if (evaluation instanceof Function) {
		return evaluation;
	}
	return () => evaluation;
}