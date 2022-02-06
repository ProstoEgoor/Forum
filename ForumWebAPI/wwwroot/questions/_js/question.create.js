import "/components/submit.button/submit.button.js";
import "/components/alert/alert.js";
import "/components/tag-list/tag-list.js";
import "/components/resizableTextArea/resizableTextArea.js";

import { getTagsRequest, createQuestionRequest } from "/questions/_js/questions.api.js";

const questionForm = document.querySelector("#question-form");
const tagList = document.querySelector('[name="tag-container"]');
const alert = document.querySelector("alert-message");
const buttonSubmit = questionForm.querySelector('button[type="submit"]');

loadTags();

function loadTags() {
	getTagsRequest()
		.then(tags => {
			tagList.tagDataList = tags.map(tag => { return tag.name });
		}).catch(() => {
			alert.showAlert("Не удалось загрузить теги.");
		})
}

questionForm.addEventListener("submit", (evt) => {
	evt.preventDefault();

	if (questionForm.submitting) {
		return;
	}

	questionForm.submitting = true;
	buttonSubmit.submitting = true;

	createQuestionRequest({
		topic: questionForm["topic"].value,
		text: questionForm["text"].value,
		tags: tagList.tags.map((tag) => { return { name: tag } })
	})
		.then((question) => {
			alert.showAlert("Вопрос успешно создан.", "success");
			window.setTimeout(() => {
				location.assign(`/questions/index.html?id=${question.id}`);
			}, 1000);
		})
		.catch((err) => {
			console.error(err);
			alert.showAlert("Не удалось создать вопрос.");
		})
		.finally(() => {
			questionForm.submitting = false;
			buttonSubmit.submitting = false;
		});
});