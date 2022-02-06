import { TagList } from "/components/tag-list/tag-list.js";

const questionTemplate = document.querySelector("#questions-template");
const questionTagTemplate = document.querySelector("#question-tag-template") || questionTemplate?.content?.querySelector("#question-tag-template");

export function createQuestion(question) {
	const newQuestion = questionTemplate.content.cloneNode(true);
	fillQuestion(newQuestion, question);
	newQuestion.questionData = question;
	return newQuestion;
}

function createQuestionTag(tag) {
	const newQuestionTag = questionTagTemplate.content.cloneNode(true);
	newQuestionTag.querySelector('*[name="tag"]').textContent = tag.name;
	return newQuestionTag;
}

export function fillQuestion(questionElement, questionData, modifiable, deleteAction) {
	const modificationIcons = questionElement.querySelector('*[name="modification-icons"]');
	if (modificationIcons && !modifiable) {
		modificationIcons.classList.add('visually-hidden');
	}

	const editLink = questionElement.querySelector('*[name="edit"]');
	if (editLink && modifiable) {
		editLink.href = `/questions/edit.html?id=${questionData.id}`;
	}

	const deleteButton = questionElement.querySelector('*[name="delete"]');
	if (deleteButton && modifiable && deleteAction) {
		deleteButton.addEventListener('click', deleteAction);
	}

	questionElement.querySelector('*[name="topic"]').textContent = questionData.topic;
	questionElement.querySelector('*[name="text"]').textContent = questionData.text;
	const tagContainer = questionElement.querySelector('*[name="tag-container"]');
	if (tagContainer instanceof TagList) {
		tagContainer.tags = questionData.tags.map((tag) => { return tag.name });
	} else {
		tagContainer.replaceChildren();
		for (let index = 0; index < questionData.tags.length; index++) {
			tagContainer.append(createQuestionTag(questionData.tags[index]));
		}
	}
	const author = questionElement.querySelector('*[name="author"]');
	if (author) {
		author.textContent = questionData.author;
	}
	const createDate = new Date(questionData.createDate);
	const createDateElement = questionElement.querySelector('*[name="createDate"]');
	if (createDateElement) {
		createDateElement.textContent = `${createDate.toLocaleString()}`;
	}
	const changeDateElement = questionElement.querySelector('*[name="changeDate"]');
	const changeDateContainer = questionElement.querySelector('*[name="changeDate-container"]');
	if (changeDateElement && questionData.changeDate) {
		const changeDate = new Date(questionData.changeDate);
		changeDateElement.textContent = `${changeDate.toLocaleString()}`;
		changeDateContainer.classList.remove('visually-hidden');
	} else if (changeDateContainer) {
		changeDateContainer.classList.add('visually-hidden');
	}

	updateAnswerCount(questionElement, questionData.answerCount);

	const descriptionLink = questionElement.querySelector('*[name="description-link"]');
	if (descriptionLink) {
		descriptionLink.href = `/questions/index.html?id=${questionData.id}`;
	}
}

export function updateAnswerCount(questionElement, answerCount) {
	const answerCountElement = questionElement.querySelector('*[name="answer-count"]');
	if (answerCountElement) {
		answerCountElement.textContent = `${answerCount} ${getNumAddition(answerCount, "Ответ", "Ответа", "Ответов")}`;
	}
}

export function getNumAddition(num, first, second, third) {
	let preLastDigit = num % 100 / 100;
	if (preLastDigit == 1) {
		return third;
	}
	num = num % 10;
	if (num == 1) {
		return first;
	} else if (num >= 2 && num <= 4) {
		return second;
	} else {
		return third;
	}
}