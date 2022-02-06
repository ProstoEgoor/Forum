import "/components/submit.button/submit.button.js";
import "/components/alert/alert.js";
import "/components/tag-list/tag-list.js";
import "/components/resizableTextArea/resizableTextArea.js";

import { getTagsRequest, getQuestionRequest, putQuestionRequest } from "/questions/_js/questions.api.js";
import { fillQuestion } from "/questions/_js/questions.utils.js";

const question = document.querySelector("#question-form");
const tagList = document.querySelector('[name="tag-container"]');
const alert = document.querySelector("alert-message");
const buttonSubmit = question.querySelector('button[type="submit"]');

loadTags();
loadQuestion();

function loadTags() {
	getTagsRequest()
		.then(tags => {
			tagList.tagDataList = tags.map(tag => { return tag.name });
		}).catch(() => {
			alert.showAlert("Не удалось загрузить теги.");
		})
}

async function loadQuestion() {
	if (question.loading) {
		return;
	}

	question.loading = true;
	buttonSubmit.submitting = true;
	buttonSubmit.lastChild.textContent = "Загрузка";

	const searchParams = new URLSearchParams(location.search);
	if (!searchParams.get("id")) {
		location.assign("/index.html");
	}

	getQuestionRequest({
		id: searchParams.get("id")
	})
		.then((questionData) => {
			question.questionData = questionData;
			fillQuestion(question, questionData);
		})
		.catch(err => {
			console.log(err);
			alert.showAlert("Не удалось загрузить вопрос.");
			window.setTimeout(() => location.assign("/index.html"), 1000);
		})
		.finally(() => {
			question.loading = false;
			buttonSubmit.submitting = false;
			buttonSubmit.lastChild.textContent = "Сохранить";
		})
}

question.addEventListener("submit", (evt) => {
	evt.preventDefault();

	if (question.submitting) {
		return;
	}

	question.submitting = true;
	buttonSubmit.submitting = true;

	putQuestionRequest(question.questionData.id, {
		topic: question["topic"].value,
		text: question["text"].value,
		tags: tagList.tags.map((tag) => { return { name: tag } })
	})
		.then(() => {
			alert.showAlert("Вопрос успешно сохранен.", "success");
			window.setTimeout(() => {
				location.assign(`/questions/index.html?id=${question.questionData.id}`);
			}, 1000);
		})
		.catch((err) => {
			console.error(err);
			alert.showAlert("Не удалось сохранить вопрос.");
		})
		.finally(() => {
			question.submitting = false;
			buttonSubmit.submitting = false;
		});
});