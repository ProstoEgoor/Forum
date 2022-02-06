import api from "/utils/api.js";


export function getTagsRequest() {
	return api.get("tags");
}

export function getQuestionRequest({ id, dateSort, ratingSort, userName }) {
	const searchParams = new URLSearchParams();
	if (dateSort != undefined) {
		searchParams.append("datesort", !!dateSort);
	} else if (ratingSort != undefined) {
		searchParams.append("ratingSort", !!ratingSort);
	}
	if (userName != undefined) {
		searchParams.append("userName", userName);
	}
	return api.get(`questions/${id}?${searchParams.toString()}`);
}

export function deleteQuestionRequest(id) {
	return api.delete(`questions/${id}`);
}

export function deleteAnswerRequest(id) {
	return api.delete(`answers/${id}`);
}

export function getQuestionsRequest({ text, tags, userName }) {
	const searchParams = new URLSearchParams();
	if (text) {
		searchParams.append("text", text);
	}
	if (tags) {
		searchParams.append("tags", tags);
	}
	if (userName) {
		searchParams.append("username", userName);
	}
	return api.get(`questions?${searchParams.toString()}`);
}

export function createQuestionRequest(question) {
	return api.post("questions", question);
}

export function putQuestionRequest(id, question) {
	return api.put(`questions/${id}`, question);
}

export function getAnswersRequest({ id, dateSort, ratingSort }) {
	const searchParams = new URLSearchParams();
	if (dateSort != undefined) {
		searchParams.append("datesort", !!dateSort);
	} else if (ratingSort != undefined) {
		searchParams.append("ratingSort", !!ratingSort);
	}
	return api.get(`questions/${id}/answers?${searchParams.toString()}`);
}


export function voteRequest(id, vote) {
	const searchParams = new URLSearchParams();
	if (vote === true) {
		searchParams.append("vote", "true");
	} else if (vote === false) {
		searchParams.append("vote", "false");
	}

	return api.post(`answers/${id}/vote?${searchParams.toString()}`);
}

export function createAnswerRequest(answer) {
	return api.post("answers", answer);
}

export function putAnswerRequest(id, answer) {
	return api.put(`answers/${id}`, answer);
}
