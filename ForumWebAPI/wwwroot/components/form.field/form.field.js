import {
	attributeValueToBool,
	attributeValueToFunction,
} from "/utils/forms.js";

const requiredMarkTemplate = document.createElement("span");
requiredMarkTemplate.innerHTML = "*";
requiredMarkTemplate.className = "text-danger";
requiredMarkTemplate.title = "Обязательное поле";

class FormField extends HTMLElement {
	constructor() {
		super();

		this.labelElement = document.createElement("label");
		this.labelElement.className = "form-label";
		this.append(this.labelElement);

		this.input = document.createElement("input");
		this.input.className = "form-control";
		this.input.addEventListener("change", () => this.touch());
		this.input.addEventListener("blur", () => this.touch());
		this.input.addEventListener("input", () => {
			if (this.touched) this.validate();
		});
		this.append(this.input);

		this.errorElement = document.createElement("div");
		this.errorElement.className = "invalid-feedback";
		this.append(this.errorElement);
	}

	touch() {
		this.touched = true;
		this.validate();
	}

	validate() {
		if (this.required && !this.input.value && this.input.value !== 0) {
			this.error = "Это поле является обязательным";
		} else if (this._validation) {
			const validationResult = this._validation(this.input.value);
			this.error = validationResult;
		} else {
			this.error = null;
		}
	}

	static get observedAttributes() {
		return [
			"name",
			"label",
			"placeholder",
			"type",
			"value",
			"error",
			"required",
			"validation",
			"maxlength",
			"size",
			"min",
			"max",
			"step",
			"disabled",
		];
	}

	get name() {
		return this.getAttribute("name");
	}
	set name(value) {
		this.setAttribute("name", value);
	}

	get type() {
		return this.getAttribute("type");
	}
	set type(value) {
		this.setAttribute("type", value);
	}

	get label() {
		return this.getAttribute("label");
	}
	set label(value) {
		this.setAttribute("label", value);
	}

	get value() {
		return this.getAttribute("value");
	}
	set value(_value) {
		this.setAttribute("value", _value);
	}

	get error() {
		return this.getAttribute("error");
	}
	set error(value) {
		if (value === undefined || value === null) {
			this.removeAttribute("error");
			return;
		}
		this.setAttribute("error", value);
	}

	get required() {
		return attributeValueToBool(this.getAttribute("required"));
	}
	set required(value) {
		if (typeof value == "boolean") {
			if (value) {
				this.setAttribute("required", "");
			} else {
				this.removeAttribute("required");
			}
			return;
		}
		this.setAttribute("required", value);
	}

	get placeholder() {
		return this.getAttribute("placeholder");
	}
	set placeholder(value) {
		this.setAttribute("placeholder", value);
	}

	get validation() {
		return this._validation;
	}
	set validation(value) {
		this._validation = attributeValueToFunction(value);
	}

	attributeChangedCallback(name, oldValue, newValue) {
		if (oldValue == newValue) return;
		switch (name) {
			case "name":
				this.input.name = newValue;
				this.labelElement.htmlFor = newValue;
				break;
			case "label":
				this.labelElement.innerText = newValue;
				break;
			case "value":
				this.input.value = newValue;
				break;
			case "placeholder":
				this.input.placeholder = newValue;
				break;
			case "required":
				this.changeRequired(newValue);
				break;
			case "error":
				this.changeError(newValue);
				break;
			case "validation":
				this.validation = newValue;
				break;
			default:
				this.input.setAttribute(name, newValue);
				break;
		}
	}

	changeError(newValue) {
		if (!!newValue) {
			this.input.classList.add("is-invalid");
		} else {
			this.input.classList.remove("is-invalid");
		}
		this.errorElement.hidden = !newValue;
		this.errorElement.innerText = newValue;
	}

	changeRequired(newValue) {
		newValue = attributeValueToBool(newValue);
		this.input.required = newValue || undefined;
		if (newValue) {
			this.labelElement.append(requiredMarkTemplate.cloneNode(true));
		} else if (newValue) {
			this.labelElement.lastChild.remove();
		}
	}
}

customElements.define("form-field", FormField);