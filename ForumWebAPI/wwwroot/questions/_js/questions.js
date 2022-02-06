import { insertParam } from "/utils/api.js";
import "/components/submit.button/submit.button.js";
import "/components/alert/alert.js";
import "/components/tag-list/tag-list.js";

import { getTagsRequest, getQuestionsRequest } from "/questions/_js/questions.api.js";
import { createQuestion } from "/questions/_js/questions.utils.js";

const questionContainer = document.querySelector("#questions-container");
const searchForm = document.querySelector("#search-form");
const tagList = document.querySelector("tag-list");
const searchButton = searchForm.querySelector('button[type="submit"]');
const alert = document.querySelector("alert-message");

loadSearchForm();

loadQuestions();

loadTags();

searchForm.addEventListener("submit", (evt) => {
	evt.preventDefault();
	insertParam("text", searchForm["text-search"].value);
	insertParam("tags", tagList.tags.join(","));

	loadQuestions();
});

function loadSearchForm() {
	const searchParams = new URLSearchParams(location.search);
	let textSearch = searchParams.get("text");
	if (textSearch) {
		searchForm["text-search"].value = decodeURIComponent(textSearch);
	}

	let tags = searchParams.get("tags");
	if (tags) {
		tagList.tags = decodeURIComponent(tags).split(",");
	}
}

function loadTags() {
	getTagsRequest()
		.then(tags => {
			tagList.tagDataList = tags.map(tag => { return tag.name });
		}).catch(() => {
			alert.showAlert("Не удалось загрузить теги.");
		})
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

function loadQuestions() {
	if (searchForm.loading) {
		return;
	}

	fillQuestions();
	searchForm.loading = true;
	searchButton.submitting = true;

	const searchParams = new URLSearchParams(location.search);

	getQuestionsRequest({
		text: searchParams.get("text"),
		tags: searchParams.get("tags")
	})
		.then(questions => {
			fillQuestions(questions);
		})
		.catch(err => {
			console.error(err);
			alert.showAlert("Не удалось загрузить вопросы.");
		})
		.finally(() => {
			searchForm.loading = false;
			searchButton.submitting = false;
		});
}