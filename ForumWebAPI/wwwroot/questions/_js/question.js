import "/components/submit.button/submit.button.js";
import "/components/alert/alert.js";
// import "/components/tag-list/tag-list.js";
import "/components/resizableTextArea/resizableTextArea.js";

import { insertParam } from "/utils/api.js";
import { getQuestionRequest, deleteQuestionRequest, voteRequest, getAnswersRequest, createAnswerRequest, deleteAnswerRequest, putAnswerRequest } from "/questions/_js/questions.api.js";
import { fillQuestion, updateAnswerCount } from "/questions/_js/questions.utils.js";

const header = document.querySelector("auth-header");
const question = document.querySelector("#question");
question.answering = false;
const sortForm = document.querySelector("#sort-form");
const answersContainer = document.querySelector("#asnwers-container");
const answerTemplate = document.querySelector("#answer-template");
const answerEditTemplate = document.querySelector("#answer-edit-template");
const alert = document.querySelector("alert-message");
const loadingSpinner = document.querySelector("#loading-spinner");

loadSearchParams();
header.addEventListener("loadProfile", (e) => {
	loadQuestion();
});

function loadSearchParams() {
	const searchParams = new URLSearchParams(location.search);
	if (searchParams.has("dateSort")) {
		if (searchParams.get("dateSort") === "true") {
			sortForm.sort.value = "old";
		} else {
			sortForm.sort.value = "new";
		}
	} else {
		sortForm.sort.value = "useful";
	}

	if (!searchParams.get("id")) {
		location.assign("/index.html");
	}

	question.id = +searchParams.get("id");
}


const answerButton = question.querySelector('[name="answer-button"]');
answerButton.addEventListener('click', () => {
	if (!header.profile) {
		alert.showAlert("Для ответа необходимо войти.");
		return
	}
	if (question.answering) {
		answersContainer.children[0].remove();
	} else {
		answersContainer.prepend(createAnswer({ questionId: question.id }, true));
		answersContainer.firstElementChild.classList.add("mb-5");
	}
	question.answering = !question.answering;
});


sortForm.addEventListener("change", () => {
	question.answering = false;

	if (sortForm.sort.value != "useful") {
		insertParam("dateSort", sortForm.sort.value == "old");
		insertParam("ratingSort", "");
	} else {
		insertParam("ratingSort", true);
		insertParam("dateSort", "");
	}

	loadAnswers();
});

function loadAnswers() {
	if (answersContainer.loading) {
		return;
	}

	answersContainer.replaceChildren();
	answersContainer.loading = true;
	loadingSpinner.loading = true;

	const searchParams = new URLSearchParams(location.search);
	getAnswersRequest({
		id: searchParams.get("id"),
		dateSort: searchParams.has("dateSort") ? searchParams.get("dateSort") == "true" : undefined,
		ratingSort: true
	})
		.then(answers => {
			fillAnswers(answers, header.profile);
		})
		.catch(err => {
			console.error(err);
			alert.showAlert("Не удалось загрузить ответы.");
		})
		.finally(() => {
			loadingSpinner.loading = false;
			answersContainer.loading = false;
		});
}

function loadQuestion() {
	if (answersContainer.loading) {
		return;
	}

	answersContainer.loading = true;
	question.classList.add("visually-hidden");
	loadingSpinner.loading = true;

	const searchParams = new URLSearchParams(location.search);

	getQuestionRequest({
		id: searchParams.get("id"),
		dateSort: searchParams.has("dateSort") ? searchParams.get("dateSort") == "true" : undefined,
		ratingSort: true
	})
		.then((questionData) => {
			document.title = "Forum - " + questionData.topic;
			question.questionData = questionData;
			fillQuestion(question, questionData, isModifiable(questionData.author, header.profile), () => {
				deleteQuestionRequest(questionData.id)
					.then(() => {
						alert.showAlert("Успешно удалено.", "success");
						window.setTimeout(() => location.assign("/index.html"), 1000);
					})
					.catch(err => {
						alert.showAlert("Не удалось удалить вопрос.");
						console.log(err);
					})
			});
			fillAnswers(questionData.answers, header.profile);
			question.classList.remove("visually-hidden");
		})
		.catch(err => {
			console.log(err);
			alert.showAlert("Не удалось загрузить вопрос.");
			window.setTimeout(() => location.assign("/index.html"), 1000);
		})
		.finally(() => {
			loadingSpinner.loading = false;
			answersContainer.loading = false;
		})
}

function fillAnswer(answer, answerData, modifiable, voteable) {
	answer.querySelector('*[name="text"]').textContent = answerData?.text;

	const form = answer.querySelector('form');
	if (form) {
		const submitButton = form.querySelector('button');
		form.addEventListener('submit', (evt) => {
			evt.preventDefault();
			submitButton.submitting = true;

			if (answerData.id) {
				putAnswerRequest(answerData.id, { text: form.text.value })
					.then(() => {
						alert.showAlert("Ответ сохранен.", "success");
						question.answering = false;
						loadAnswers();
					})
					.catch(() => {
						alert.showAlert("Не удалось сохранить ответ");
					})
					.finally(() => {
						submitButton.submitting = false;
					})
			} else {
				createAnswerRequest({ questionId: +answerData.questionId, text: form.text.value })
					.then(() => {
						alert.showAlert("Ответ сохранен.", "success");
						question.answering = false;
						updateAnswerCount(question, ++question.questionData.answerCount)
						loadAnswers();
					})
					.catch(() => {
						alert.showAlert("Не удалось сохранить ответ");
					})
					.finally(() => {
						submitButton.submitting = false;
					})
			}
		});
	}

	if (answerData.id) {
		const modificationIcons = answer.querySelector('*[name="modification-icons"]');
		if (modificationIcons && !modifiable) {
			modificationIcons.classList.add('visually-hidden');
		}

		const rating = answer.querySelector('*[name="rating"]');
		if (rating) {
			rating.textContent = answerData.rating;
		}

		const upRatingButton = answer.querySelector('*[name="upRating-button"]');
		if (upRatingButton) {
			if (answerData.myVote === true) {
				upRatingButton.innerHTML = '<i class="bi bi-caret-up-fill"></i>';
			} else {
				upRatingButton.innerHTML = '<i class="bi bi-caret-up"></i>';
			}

			upRatingButton.addEventListener("click", () => {
				if (!voteable) {
					alert.showAlert("Для голосования необходимо войти.");
					return
				}

				let vote = null;
				if (answerData.myVote !== true) {
					vote = true;
				}

				voteRequest(answerData.id, vote)
					.then(() => {
						if (vote === true) {
							answerData.rating++;
							upRatingButton.innerHTML = '<i class="bi bi-caret-up-fill"></i>';

						} else {
							answerData.rating--;
							upRatingButton.innerHTML = '<i class="bi bi-caret-up"></i>';
						}

						if (answerData.myVote === false) {
							answerData.rating++;
							downRatingButton.innerHTML = '<i class="bi bi-caret-down"></i>';
						}

						answerData.myVote = vote;
						rating.textContent = answerData.rating;
					})
					.catch((err) => {
						alert.showAlert("Не удалось проголосовать.");
					})
			});
		}

		const downRatingButton = answer.querySelector('*[name="downRating-button"]');
		if (downRatingButton) {
			if (answerData.myVote === false) {
				downRatingButton.innerHTML = '<i class="bi bi-caret-down-fill"></i>';
			} else {
				downRatingButton.innerHTML = '<i class="bi bi-caret-down"></i>';
			}

			downRatingButton.addEventListener("click", () => {
				if (!voteable) {
					alert.showAlert("Для голосования необходимо войти.");
					return
				}

				let vote = null;
				if (answerData.myVote !== false) {
					vote = false;
				}

				voteRequest(answerData.id, vote)
					.then(() => {
						if (vote === false) {
							answerData.rating--;
							downRatingButton.innerHTML = '<i class="bi bi-caret-down-fill"></i>';
						} else {
							answerData.rating++;
							downRatingButton.innerHTML = '<i class="bi bi-caret-down"></i>';
						}

						if (answerData.myVote === true) {
							answerData.rating--;
							upRatingButton.innerHTML = '<i class="bi bi-caret-up"></i>';
						}

						answerData.myVote = vote;
						rating.textContent = answerData.rating;
					})
					.catch((err) => {
						alert.showAlert("Не удалось проголосовать.");
					})
			});
		}

		const author = answer.querySelector('*[name="author"]');
		if (author) {
			author.textContent = answerData.author;
		}

		const createDateElement = answer.querySelector('*[name="createDate"]');
		if (createDateElement) {
			createDateElement.textContent = `${new Date(answerData.createDate).toLocaleString()}`;
		}

		const changeDateElement = answer.querySelector('*[name="changeDate"]');
		const changeDateContainer = answer.querySelector('*[name="changeDate-container"]');
		if (changeDateElement && answerData.changeDate) {
			const changeDate = new Date(answerData.changeDate);
			changeDateElement.textContent = `${changeDate.toLocaleString()}`;
			changeDateContainer.classList.remove('visually-hidden');
		} else if (changeDateContainer) {
			changeDateContainer.classList.add('visually-hidden');
		}
	}
}

function fillAnswers(answers, profile) {
	answersContainer.replaceChildren();
	answers.forEach(answer => {
		answersContainer.append(createAnswer(answer, false, profile));
		let newAnswer = answersContainer.lastElementChild;
		newAnswer.editing = false;

		let editButton = newAnswer.querySelector('*[name="edit"]');
		if (editButton) {
			editButton.addEventListener('click', () => {
				if (newAnswer.editing) {
					newAnswer.previousElementSibling.remove();
					newAnswer.classList.remove("rounded-0", "rounded-bottom", "border-top-0");
				} else {
					newAnswer.before(createAnswer(answer, true));
					newAnswer.previousElementSibling.classList.add("rounded-0", "rounded-top");
					newAnswer.classList.add("rounded-0", "rounded-bottom", "border-top-0");
				}
				newAnswer.editing = !newAnswer.editing;
			});
		}
		let deleteButton = newAnswer.querySelector('*[name="delete"]');
		if (deleteButton) {
			deleteButton.addEventListener('click', () => {
				deleteButton.disabled = true;
				deleteAnswerRequest(answer.id)
					.then(() => {
						alert.showAlert("Ответ успешно удален.", "success");
						if (newAnswer.editing) {
							newAnswer.previousElementSibling.remove();
						}
						newAnswer.remove();
						updateAnswerCount(question, --question.questionData.answerCount)
					})
					.catch(() => {
						alert.showAlert("Не удалось удалить ответ.");
					})
					.finally(() => {
						deleteButton.disabled = false;
					})
			});
		}
	});
}

function createAnswer(answer, edit, profile) {
	const newAnswer = !edit ? answerTemplate.content.cloneNode(true) : answerEditTemplate.content.cloneNode(true);
	fillAnswer(newAnswer, answer, isModifiable(answer?.author, profile), !!profile);
	newAnswer.answerData = answer;
	return newAnswer;
}

function isModifiable(author, profile) {
	if (!profile)
		return false;
	return author == profile.userName || profile.roles.includes("Moderator");
}