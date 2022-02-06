import { attributeValueToBool } from "/utils/forms.js";
import LoadingElement from "/components/loading/loading.js";

export class SubmitButton extends HTMLButtonElement {
	constructor() {
		super();
		this.loadingSpinner = new LoadingElement();
		this.loadingSpinner.addClass('me-1');
		this.prepend(this.loadingSpinner);
	}

	connectedCallback() {
		this.type = "submit";
	}

	static get observedAttributes() {
		return [...LoadingElement.observedAttributes, "submitting"];
	}
	get submitting() {
		return this.getAttribute("submitting");
	}
	set submitting(value) {
		this.setAttribute("submitting", value);
	}

	attributeChangedCallback(name, oldValue, newValue) {
		this.loadingSpinner.attributeChangedCallback(name, oldValue, newValue);
		if (name == "submitting") {
			oldValue = attributeValueToBool(oldValue);
			newValue = attributeValueToBool(newValue);
			if (oldValue == newValue) return;
			this.loadingSpinner.attributeChangedCallback("loading", oldValue, newValue);
			this.disabled = newValue;
		}
	}
}

customElements.define("button-submit", SubmitButton, { extends: "button" });