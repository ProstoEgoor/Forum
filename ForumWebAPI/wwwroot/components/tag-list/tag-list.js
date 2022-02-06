export class TagList extends HTMLElement {
	constructor() {
		super();

		this.tagList = [];

		const row = document.createElement("div");
		row.classList.add("row", "align-items-center", "gy-3");

		let col = document.createElement("div");
		col.classList.add("col-12", "col-sm-auto");
		row.append(col);

		const inputGroup = document.createElement("div");
		inputGroup.classList.add("input-group");
		col.append(inputGroup);

		this.input = document.createElement("input");
		this.input.classList.add("form-control");
		this.input.setAttribute("placeholder", "Искать тег...");
		this.input.setAttribute("list", "tag-datalist");
		this.input.setAttribute("aria-describedby", "button-add-tag");
		this.input.setAttribute("autocomplete", "off");
		inputGroup.append(this.input);

		this.datalist = document.createElement("datalist");
		this.datalist.setAttribute("id", "tag-datalist");
		inputGroup.append(this.datalist);

		this.addButton = document.createElement("button");
		this.addButton.classList.add("btn", "btn-outline-secondary");
		this.addButton.setAttribute("type", "button");
		this.addButton.setAttribute("id", "button-add-tag");
		this.addButton.textContent = "Добавить";
		this.addButton.addEventListener("click", () => {
			this.addTag(this.input.value);
			this.input.value = "";
		})
		inputGroup.append(this.addButton);

		col = document.createElement("div");
		col.classList.add("col-12", "col-sm-auto");
		row.append(col);

		this.tagContainer = document.createElement("ul");
		this.tagContainer.classList.add("list-group", "list-group-horizontal", "flex-wrap", "gap-2");
		col.append(this.tagContainer);

		this.replaceChildren(row);
	}

	createTag(name) {
		const li = document.createElement("li");
		li.classList.add("list-group-item", "d-flex", "align-items-baseline", "border-1", "rounded-pill", "py-0", "pe-1");

		const span = document.createElement("span");
		span.classList.add("me-2");
		span.textContent = name;
		li.append(span);

		const button = document.createElement("button");
		button.setAttribute("type", "button");
		button.classList.add("link-secondary-dark", "btn-reset");
		button.addEventListener("click", () => {
			this.removeTag(name);
			li.remove();
		});
		li.append(button);

		const iconClose = document.createElement("i");
		iconClose.classList.add("bi", "bi-x-circle");
		button.append(iconClose);

		return li;
	}

	addTag(name) {
		if (name && !this.tagList.includes(name)) {
			this.tagList.push(name);
			this.tagContainer.append(this.createTag(name));
		}
	}

	removeTag(name) {
		const index = this.tagList.indexOf(name);
		if (index !== -1) {
			this.tagList.splice(index, 1);
		}
	}

	get tags() {
		return this.tagList;
	}

	set tags(tags) {
		if (!Array.isArray(tags)) {
			tags = [tags];
		}

		this.tagList.length = 0;
		this.tagContainer.replaceChildren();

		tags.forEach(tag => {
			this.addTag(tag);
		});
	}

	set tagDataList(tags) {
		this.datalist.replaceChildren();
		tags.forEach(tag => {
			let option = document.createElement("option");
			option.setAttribute("value", tag);
			this.datalist.append(option);
		});
	}

	connectedCallback() {

	}
}

customElements.define("tag-list", TagList);