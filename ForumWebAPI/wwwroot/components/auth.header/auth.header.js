import api from "/utils/api.js";
import { attributeValueToBool } from "/utils/forms.js";
import LoadingElement from "/components/loading/loading.js";

function logoutRequest() {
	return api.post("account/logout");
}

function getProfile() {
	return api.get("account");
}

//<li><a href="..." class="dropdown-item" type="button">...</a></li>
function createMenuListItem(content, link, action) {
	const menuItem = document.createElement("li");

	if (link) {
		const menuItemLink = document.createElement("a");
		menuItemLink.classList.add("dropdown-item");
		menuItemLink.innerHTML = content;
		menuItemLink.href = link;
		if (action) {
			menuItemLink.addEventListener("click", action);
		}
		menuItem.append(menuItemLink);
	} else {
		menuItem.innerHTML = content;
	}

	return menuItem;
}

//	<ul class="dropdown-menu dropdown-menu-end" aria-labelledby="profileMenu">
// 		<li><a class="dropdown-item" href="profile/index.html">Профиль</a></li>
// 		<li><a class="dropdown-item" href="questions/user_questions.html">Мои вопросы</a></li>
// 		<li><hr class="dropdown-divider"></li>
// 		<li><a class="dropdown-item" href="#">Выйти</a></li>
//	</ul>
function createUserMenu(logoutAction) {
	const menuList = document.createElement("ul");
	menuList.classList.add("dropdown-menu", "dropdown-menu-end");
	menuList.setAttribute("aria-labelledby", "profileMenu");

	menuList.append(createMenuListItem("Профиль", "/profile/index.html"));
	menuList.append(createMenuListItem("Мои вопросы", "/profile/questions.html"));
	menuList.append(createMenuListItem('<hr class="dropdown-divider">'));
	menuList.append(createMenuListItem("Выйти", "#", logoutAction));
	return menuList;
}

//	<div class="dropdown order-3 order-md-4 ms-1">
// 		<button class="btn btn-success dropdown-toggle fs-5" type="button" id="profileMenu" data-bs-toggle="dropdown" aria-expanded="false">
//			<i class="bi bi-person-circle me-1"></i>
//			<span id="authHeader-userName">UserName</span>
//		</button>
//    ...
//	</div>
function createUserDropdown(userName, logoutAction) {
	const dropdown = document.createElement("div");
	dropdown.classList.add("dropdown", "order-3", "order-md-4", "ms-1");

	const dropdownButton = document.createElement("button");
	dropdownButton.classList.add("btn", "btn-success", "dropdown-toggle", "fs-5");
	dropdownButton.setAttribute("type", "button");
	dropdownButton.setAttribute("id", "profileMenu");
	dropdownButton.setAttribute("data-bs-toggle", "dropdown");
	dropdownButton.setAttribute("aria-expanded", "false");
	dropdown.append(dropdownButton);

	const dropdownIcon = document.createElement("i");
	dropdownIcon.classList.add("bi", "bi-person-circle", "me-1");
	dropdownButton.append(dropdownIcon);

	const userNameSpan = document.createElement("span");
	userNameSpan.setAttribute("id", "authHeader-userName");
	userNameSpan.textContent = userName;
	dropdownButton.append(userNameSpan);

	dropdown.append(createUserMenu(logoutAction));
	return dropdown;
}

//	<li class="nav-item">
//		<a class="nav-link" href="...">...</a>
//	</li>
function createNavListItem(content, link, action) {
	const menuItem = document.createElement("li");
	menuItem.classList.add("nav-item");

	if (link) {
		const menuItemLink = document.createElement("a");
		menuItemLink.classList.add("nav-link");
		menuItemLink.innerHTML = content;
		menuItemLink.href = link;
		if (action) {
			menuItemLink.addEventListener("click", action);
		}
		menuItem.append(menuItemLink);
	} else {
		menuItem.innerHTML = content;
	}

	return menuItem;
}

//	<nav class="order-4 order-md-3 ms-auto">
// 		<ul class="navbar-nav align-items-center gap-2">
//			<li class="nav-item">
// 				<a class="nav-link" href="/tags/index.html">Теги</a>
// 			</li>
// 			<li class="nav-item">
// 				<a class="nav-link" href="/users/index.html">Пользователи</a>
// 			</li>
// 			<li class="nav-item">
// 				<a class="nav-link" href="/questions/create.html">Задать вопрос</a>
// 			</li>
// 			<li class="nav-item">
// 				<a class="nav-link" href="#">Войти</a>
// 			</li>
// 			<li class="nav-item">
// 				<a class="nav-link" href="#">Зарегистрироваться</a>
// 			</li>
// 		</ul>
// 	</nav>
function createNavigationMenu({ isAuthenticated, isUser, isAdmin, isLoginPage, isRegisterPage, loginUrl, registerUrl }) {
	const nav = document.createElement("nav");
	nav.classList.add("order-4", "order-md-3", "ms-auto");

	const navList = document.createElement("ul");
	navList.classList.add("navbar-nav", "align-items-center", "gap-2");
	nav.append(navList);

	if (isAuthenticated) {
		if (isAdmin) {
			navList.append(createNavListItem("Теги", "/tags/index.html"));
			navList.append(createNavListItem("Пользователи", "/users/index.html"));
		}

		if (isUser) {
			navList.append(createNavListItem("Задать вопрос", "/questions/create.html"));
		}
	} else {
		if (!attributeValueToBool(isLoginPage)) {
			navList.append(createNavListItem("Войти", loginUrl));
		}
		if (!attributeValueToBool(isRegisterPage)) {
			navList.append(createNavListItem("Зарегистрироваться", "/profile/create.html"));
		}
	}

	return nav;
}

//<header class="navbar navbar-expand-lg navbar-dark bg-success">
//	<div class="container">
//		<a href="index.html" class="navbar-brand mb-1">Форум</a>
//		<span class="navbar-text me-auto">...</span>
//		...
//		<div class="text-secondary visually-hidden ms-auto">
//			<span class="spinner-border"></span>
//		</div>
//</header>
export class AuthHeader extends LoadingElement {
	constructor() {
		super();
		this.authHeader = document.createElement("header");
		this.authHeader.classList.add("navbar", "navbar-expand-lg", "navbar-dark", "bg-success");

		const container = document.createElement("div");
		container.classList.add("container");
		this.authHeader.append(container);

		const indexLink = document.createElement("a");
		indexLink.classList.add("navbar-brand", "mb-1");
		indexLink.href = "/index.html";
		indexLink.textContent = "Форум";
		container.append(indexLink);

		this.authHeader.pageTitle = document.createElement("span");
		this.authHeader.pageTitle.classList.add("navbar-text", "me-auto", "text-light");
		this.authHeader.pageTitle.textContent = this.getAttribute("page-title");
		container.append(this.authHeader.pageTitle);

		container.append(this.loadingSpinner);
		super.addClass("ms-auto");
		// super.setAttribute("spinner-color", "primary");

		this.replaceChildren(this.authHeader);
	}

	shouldAnonymous = [
		/^\/profile\/create.html$/
	];

	canAnonymous = [
		/^\/auth\/login.html/,
		/^\/profile\/create.html$/,
		/^\/index.html/,
		/^\/questions\/index.html/
	];

	testPathname(urls, url) {
		return urls.some((allowUrl) => allowUrl.test(url));
	}

	static get observedAttributes() {
		return [
			...super.observedAttributes,
			"page-title",
			"login-page",
			"register-page"
		];
	}

	attributeChangedCallback(name, oldValue, newValue) {
		super.attributeChangedCallback(name, oldValue, newValue);
		switch (name) {
			case "page-title":
				this.authHeader.pageTitle.textContent = newValue;
				break;
		}
	}

	logout() {
		console.log("logout");

		logoutRequest().finally(() => {
			this.render();
			delete this.profile;
		});
	}

	connectedCallback() {
		this.render();
	}

	render() {
		this.loading = true;
		this.authHeader.nav?.remove();
		this.authHeader.dropdown?.remove();

		if (this.testPathname(this.shouldAnonymous, location.pathname)) {
			this.loginUrl = "/auth/login.html";
			this.registerUrl = "/profile/create.html";
		} else {
			this.loginUrl = `/auth/login.html?from=${encodeURIComponent(location.pathname + location.search)}`;
			this.registerUrl = `/profile/create.html?from=${encodeURIComponent(location.pathname + location.search)}}`;
		}

		new Promise((resolve, reject) => {
			if (attributeValueToBool(this.getAttribute("login-page")) || attributeValueToBool(this.getAttribute("register-page"))) {
				reject("unauthenticated");
			} else {
				resolve(getProfile());
			}
		})
			.then(profile => {
				this.profile = profile;
			})
			.catch(() => { })
			.finally(() => {
				if (!this.profile && !this.testPathname(this.canAnonymous, location.pathname)) {
					location.assign("/index.html");
				}

				this.dispatchEvent(new CustomEvent('loadProfile', { detail: this.profile }));
				this.authHeader.nav = createNavigationMenu({
					isAuthenticated: !!this.profile,
					isUser: this.profile?.roles.includes("User"),
					isAdmin: this.profile?.roles.includes("Admin"),
					isLoginPage: attributeValueToBool(this.getAttribute("login-page")),
					isRegisterPage: attributeValueToBool(this.getAttribute("register-page")),
					loginUrl: this.loginUrl,
					registerUrl: this.registerUrl
				});
				this.authHeader.pageTitle.after(this.authHeader.nav);

				if (this.profile) {
					this.authHeader.dropdown = createUserDropdown(this.profile.userName, () => this.logout());
					this.authHeader.nav.after(this.authHeader.dropdown);
				}
			});
		this.loading = false;
	}
}

customElements.define("auth-header", AuthHeader);