export default class LoadingElement extends HTMLElement {
	constructor() {
		super();
		this.loadingSpinner = document.createElement("div");
		this.loadingSpinner.classList.add("spinner-border");
		this.loadingSpinner.setAttribute("role", "status");
		this.changeState(null, this.loading);

		this.replaceChildren(this.loadingSpinner);

		this._availableColors = ["primary", "secondary", "danger", "warning", "light", "dark"];
	}

	addClass(className) {
		this.loadingSpinner.classList.add(className);
	}

	static get observedAttributes() {
		return ["loading", "spinner-color", "spinner-small"];
	}

	get loading() {
		return this.getAttribute("loading") === "" || !!this.getAttribute("loading");
	}

	set loading(value) {
		if (!!value) {
			this.setAttribute("loading", "");
		} else {
			this.removeAttribute("loading");
		}
	}

	get color() {
		return this.getAttribute("spinner-color");
	}
	set color(value) {
		this.setAttribute("spinner-color", value);
	}

	attributeChangedCallback(name, oldValue, newValue) {
		switch (name) {
			case "loading":
				this.changeState(oldValue, newValue);
				break;
			case "spinner-color":
				this.changeColor(oldValue, newValue);
				break;
			case "spinner-small":
				this.setSmallSize(oldValue, newValue);
				break;
		}
	}

	changeState(oldValue, newValue) {
		if (oldValue != newValue) {
			if (newValue === "" || !!newValue) {
				this.loadingSpinner.classList.remove("visually-hidden");
			} else {
				this.loadingSpinner.classList.add("visually-hidden");
			}
		}
	}

	changeColor(oldValue, newValue) {
		if (oldValue != newValue) {
			if (this._availableColors.includes(oldValue)) {
				this.loadingSpinner.classList.remove("text-" + oldValue);
			} else {
				this.loadingSpinner.style.removeProperty("color");
			}

			if (this._availableColors.includes(newValue)) {
				this.loadingSpinner.classList.add("text-" + newValue);
			} else {
				this.loadingSpinner.style.color = newValue;
			}
		}
	}

	setSmallSize(oldValue, newValue) {
		if (oldValue != newValue) {
			if (newValue === "" || !!newValue) {
				this.loadingSpinner.classList.add("spinner-border-sm");
			} else {
				this.loadingSpinner.classList.remove("spinner-border-sm");
			}
		}
	}
}

customElements.define("loading-element", LoadingElement);