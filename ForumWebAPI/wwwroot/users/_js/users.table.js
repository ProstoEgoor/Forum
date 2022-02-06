import { getUsersRequest, deleteUserRequest } from "./user.api.js";
// import { HttpStatusError } from "/utils/api.js";
import "/components/alert/alert.js";
import "/components/loading/loading.js";

const table = document.querySelector("#table-users");
const rowTemplate = document.querySelector("#template-users");
const loadingSpinner = document.querySelector("#loading-spinner");
const alert = document.querySelector("alert-message");

loadUsers();

function loadUsers() {
	const tableBody = table.tBodies[0];

	loadingSpinner.loading = true;

	getUsersRequest()
		.then((users) => {
			tableBody.innerHTML = "";
			for (let user of users) {
				tableBody.append(createTableRow(user));
			}
		})
		.catch((err) => {
			console.error(err);
			tableBody.innerHTML = "";
			alert.showAlert("Не удалось загрузить список пользователей.");
		})
		.finally(() => {
			loadingSpinner.loading = false;
		})
}

function createTableRow(user) {
	const row = rowTemplate.content.cloneNode(true);
	for (let key in user) {
		const cell = row.querySelector(`[name="${key}"]`);
		if (cell) {
			cell.innerHTML = user[key];
		}
	}

	const questionsButton = row.querySelector('[name="questions"]');
	if (questionsButton) {
		questionsButton.href = "/profile/questions.html?userName=" + user.userName;
	}
	const editButton = row.querySelector('[name="edit"]');
	if (editButton) {
		editButton.href = "./edit.html?userName=" + user.userName;
	}
	const deleteButton = row.querySelector('[name="delete"]');
	if (deleteButton) {
		deleteButton.onclick = () => deleteUser(user);
	}
	return row;
}

function deleteUser(user) {
	deleteUserRequest(user)
		.then(() => loadUsers())
		.catch(() => alert.showAlert(`Не удалось удалить пользователя ${user.userName}`));
}