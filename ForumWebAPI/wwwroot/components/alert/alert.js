import { attributeValueToBool } from "/utils/forms.js";

export class AlertMessage extends HTMLElement {
	connectedCallback() {
		this.classList.add("alert");
	}

	static get observedAttributes() {
		return ["color", "timeout", "show"];
	}

	get color() {
		return this.getAttribute(color);
	}
	set color(value) {
		this.setAttribute("color", value);
	}

	get timeout() {
		return this.getAttribute("timeout");
	}
	set timeout(value) {
		this.setAttribute("timeout", value);
	}

	get show() {
		return attributeValueToBool(this.getAttribute("show"));
	}
	set show(value) {
		if (!!value) {
			this.setAttribute("show", "");
		} else {
			this.removeAttribute("show");
		}
	}

	attributeChangedCallback(name, oldValue, newValue) {
		switch (name) {
			case "color":
				this.changeColor(oldValue, newValue);
				break;
			case "timeout":
				this.timeout = newValue;
				break;
			case "show":
				if (newValue === "" || !!newValue) {
					this.classList.add("show", "my-1");
					window.scroll(0, this.getBoundingClientRect().top - window.innerHeight);
				} else {
					this.classList.remove("show", "my-1");
				}
				break;
		}
	}

	changeColor(oldValue, newValue) {
		if (oldValue != newValue) {
			this.classList.remove("alert-" + oldValue);
			this.style.color = null;
			if (["danger", "warning", "info", "success"].includes(newValue)) {
				this.classList.add("alert-" + newValue);
			} else {
				this.style.color = color;
			}
		}
	}

	hideAlert() {
		this.textContent = "";
		this.show = false;
		window.clearTimeout(this.firstTimeOut);
		window.clearTimeout(this.secondTimeOut);
	}

	showAlert(message, color = "danger") {
		if (message) {
			if (this.show) {
				this.textContent += '\n' + message;
			} else {
				this.textContent = message;
			}
			this.color = color;
			this.show = true;

			this.firstTimeOut = window.setTimeout(() => { this.show = false }, this.timeout || 3000);
			this.secondTimeOut = window.setTimeout(() => this.hideAlert(), this.timeout || 3150);
		}
	}
}

customElements.define("alert-message", AlertMessage);