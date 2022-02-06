import api from "/utils/api.js";
import "/components/alert/alert.js";
import "/components/loading/loading.js";

const table = document.querySelector("#table-tags");
const rowTemplate = document.querySelector("#template-tag");
const loadingSpinner = document.querySelector("#loading-spinner");
const alert = document.querySelector("alert-message");

function getTagsRequest() {
	return api.get("tags");
}

loadTags();

function loadTags() {
	const tableBody = table.tBodies[0];

	loadingSpinner.loading = true;
	getTagsRequest()
		.then((tags) => {
			tableBody.innerHTML = "";
			for (let tag of tags) {
				tableBody.append(createTableRow(tag));
			}
		})
		.catch((err) => {
			console.error(err);
			tableBody.innerHTML = "";
			alert.showAlert("Не удалось загрузить теги.");
		})
		.finally(() => {
			loadingSpinner.loading = false;
		})

}

function createTableRow(tag) {
	const row = rowTemplate.content.cloneNode(true);
	for (let key in tag) {
		const cell = row.querySelector(`[name="${key}"]`);
		if (cell) {
			cell.innerHTML = tag[key];
		}
	}
	return row;
}