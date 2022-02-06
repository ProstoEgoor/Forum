import { attributeValueToBool } from "/utils/forms.js";

export class ResizableTextArea extends HTMLTextAreaElement {
	constructor() {
		super();
		this.setAttribute("style", "overflow-y:hidden;");
	}

	connectedCallback() {
		this._onInput();
	}

	static get observedAttributes() {
		return ["autoresize"];
	}

	_onInput() {
		this.style.height = "auto";
		this.style.height = (this.scrollHeight + 2) + "px";
	}

	attributeChangedCallback(name, oldValue, newValue) {
		if (name == "autoresize") {
			oldValue = attributeValueToBool(oldValue);
			newValue = attributeValueToBool(newValue);
			if (oldValue == newValue) return;
			if (newValue) {
				this.addEventListener("input", this._onInput);
			} else {
				this.removeEventListener("input", this._onInput);
			}
		}
	}
}

customElements.define("resizable-textarea", ResizableTextArea, { extends: "textarea" });