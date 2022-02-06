import api from "/utils/api.js";
export { HttpStatusError } from "/utils/api.js";

export function getUsersRequest() {
	return api.get("users");
}

export function getUserRequest(userName) {
	return api.get("users/" + userName);
}

export function updateUserRequest(user) {
	const url = `users/${user.userName}`;
	delete user.userName;
	return api.put(url, user);
}

export function deleteUserRequest(user) {
	return api.delete("users/" + user.userName);
}

export function addUserRoleRequest(user, role) {
	return api.post(`users/${user.userName}/roles?role=${role}`);
}

export function deleteUserRoleRequest(user, role) {
	return api.delete(`users/${user.userName}/roles?role=${role}`);
}

export function setRolesRequest(user, roles) {
	return api.post(`users/${user.userName}?roles=${roles.join(',')}`);
}

export function resetUserPasswordRequest(user, password) {
	return api.put(`users/${user.userName}/password`, { newPassword: password });
}