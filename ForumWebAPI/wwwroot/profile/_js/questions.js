import { insertParam } from "/utils/api.js";
import "/components/alert/alert.js";

import { getQuestionsRequest } from "/questions/_js/questions.api.js";
import { createQuestion } from "/questions/_js/questions.utils.js";

const header = document.querySelector("auth-header");
const questionContainer = document.querySelector("#questions-container");
const loadingSpinner = document.querySelector("#loading-spinner");
const alert = document.querySelector("alert-message");

header.addEventListener("loadProfile", (e) => {
	loadQuestions();
})

async function loadQuestions() {
	if (questionContainer.loading) {
		return;
	}

	questionContainer.loading = true;
	loadingSpinner.loading = true;

	const searchParams = new URLSearchParams(location.search);

	if (searchParams.has("userName") && searchParams.get("userName") != header.profile?.userName) {
		document.title = "Forum - вопросы " + searchParams.get("userName");
		header.setAttribute("page-title", `Вопросы ${searchParams.get("userName")}`);
	}

	const currentUserName = searchParams.get("userName") || header.profile?.userName;

	getQuestionsRequest({
		userName: currentUserName
	})
		.then(questions => {
			if (!questions.length) {
				alert.showAlert(`У пользователя ${currentUserName} не найдено вопросов.`, "success");
				setTimeout(() => { history.back() }, 1500);
			} else {
				fillQuestions(questions);
			}
		})
		.catch(err => {
			console.error(err);
			alert.showAlert(`Не удалось загрузить вопросы пользователя ${currentUserName}.`);
			setTimeout(() => { history.back() }, 1500);
		})
		.finally(() => {
			questionContainer.loading = false;
			loadingSpinner.loading = false;
		});
}

function fillQuestions(questions) {
	if (questions) {
		for (const question of questions) {
			questionContainer.append(createQuestion(question));
		}
	} else {
		questionContainer.replaceChildren();
	}
}