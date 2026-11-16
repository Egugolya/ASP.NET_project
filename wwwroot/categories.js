const categoryForm = document.querySelector("#category-form");
const categoryNameInput = document.querySelector("#category-name");
const formTitle = document.querySelector("#form-title");
const submitCategoryButton = document.querySelector("#submit-category");
const cancelEditButton = document.querySelector("#cancel-edit");
const categoryStatus = document.querySelector("#category-status");
const categoryCount = document.querySelector("#category-count");
const categoryFilter = document.querySelector("#category-filter");
const categorySearchButton = document.querySelector("#category-search");
const categorySort = document.querySelector("#category-sort");
const categoriesList = document.querySelector("#categories-list");
const emptyState = document.querySelector("#empty-state");
const previousPageButton = document.querySelector("#previous-page");
const nextPageButton = document.querySelector("#next-page");
const pageInfo = document.querySelector("#page-info");

const pageSize = 5;
let categories = [];
let editingCategoryId = null;
let currentPage = 1;
let totalPages = 1;
let totalCount = 0;
let loadController;

categoryForm.addEventListener("submit", async (event) => {
    event.preventDefault();
    const name = categoryNameInput.value.trim();
    const isEditing = editingCategoryId !== null;

    try {
        const response = await fetch(isEditing ? `/api/categories/${editingCategoryId}` : "/api/categories", {
            method: isEditing ? "PUT" : "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ name })
        });
        if (!response.ok) {
            categoryStatus.textContent = await getErrorMessage(response, isEditing ? "Could not update category" : "Could not add category");
            return;
        }

        resetCategoryForm();
        if (!isEditing)
            currentPage = 1;
        const loaded = await loadCategories();
        categoryStatus.textContent = loaded ? (isEditing ? "Category updated." : "Category added.") : "Category saved, but the list could not be refreshed.";
    } catch (error) {
        categoryStatus.textContent = `Request error: ${error.message}`;
    }
});

cancelEditButton.addEventListener("click", resetCategoryForm);
categorySearchButton.addEventListener("click", () => {
    currentPage = 1;
    loadCategories();
});
categorySort.addEventListener("change", () => {
    currentPage = 1;
    loadCategories();
});
previousPageButton.addEventListener("click", () => {
    if (currentPage > 1) {
        currentPage--;
        loadCategories();
    }
});
nextPageButton.addEventListener("click", () => {
    if (currentPage < totalPages) {
        currentPage++;
        loadCategories();
    }
});

function resetCategoryForm() {
    editingCategoryId = null;
    categoryForm.reset();
    formTitle.textContent = "Add a category";
    submitCategoryButton.textContent = "Add category";
    cancelEditButton.hidden = true;
}

async function loadCategories() {
    categoryStatus.textContent = "";
    loadController?.abort();
    loadController = new AbortController();

    const query = new URLSearchParams({
        page: String(currentPage),
        pageSize: String(pageSize),
        orderBy: categorySort.value
    });
    const filterName = categoryFilter.value.trim();
    if (filterName)
        query.set("filter.name", filterName);

    try {
        const response = await fetch(`/api/categories?${query}`, {
            cache: "no-store",
            signal: loadController.signal
        });
        if (!response.ok) {
            categoryStatus.textContent = await getErrorMessage(response, "Could not load categories");
            return false;
        }

        const result = await response.json();
        categories = result.models ?? result.Models ?? result.items ?? result.Items ?? [];
        totalCount = result.totalCount ?? result.TotalCount ?? 0;
        totalPages = Math.max(1, result.totalPages ?? result.TotalPages ?? Math.ceil(totalCount / pageSize));
        if (currentPage > totalPages) {
            currentPage = totalPages;
            return await loadCategories();
        }
        renderCategories();
        return true;
    } catch (error) {
        if (error.name === "AbortError")
            return false;
        categoryStatus.textContent = `Request error: ${error.message}`;
        return false;
    }
}

function renderCategories() {
    categoriesList.replaceChildren();
    categoryCount.textContent = `(${totalCount})`;
    emptyState.hidden = categories.length > 0;
    emptyState.textContent = totalCount === 0 && !categoryFilter.value.trim()
        ? "No categories yet."
        : "No categories match this filter.";
    pageInfo.textContent = `Page ${currentPage} of ${totalPages}`;
    previousPageButton.disabled = currentPage <= 1;
    nextPageButton.disabled = currentPage >= totalPages;

    for (const category of categories) {
        categoriesList.append(createCategoryRow(category));
    }
}

function createCategoryRow(category) {
    const id = category.id ?? category.Id;
    const name = getName(category);
    const row = document.createElement("tr");
    row.append(
        createCell(id),
        createCell(name),
        createCell(formatDate(category.createdAt ?? category.creationDate ?? category.CreatedAt)),
        createCell(formatDate(category.updatedAt ?? category.updateDate ?? category.UpdatedAt))
    );

    const actions = document.createElement("td");
    actions.className = "actions";
    const editButton = document.createElement("button");
    editButton.type = "button";
    editButton.textContent = "Edit";
    editButton.addEventListener("click", () => beginEdit(id, name));

    const deleteButton = document.createElement("button");
    deleteButton.type = "button";
    deleteButton.className = "delete-button";
    deleteButton.textContent = "Delete";
    deleteButton.addEventListener("click", () => deleteCategory(id, name));

    actions.append(editButton, deleteButton);
    row.append(actions);
    return row;
}

function createCell(value) {
    const cell = document.createElement("td");
    cell.textContent = value ?? "";
    return cell;
}

function beginEdit(id, name) {
    editingCategoryId = id;
    categoryNameInput.value = name;
    formTitle.textContent = "Edit category";
    submitCategoryButton.textContent = "Save changes";
    cancelEditButton.hidden = false;
    categoryStatus.textContent = "";
    categoryNameInput.focus();
    window.scrollTo({ top: 0, behavior: "smooth" });
}

async function deleteCategory(id, name) {
    if (!window.confirm(`Delete “${name}”?`)) {
        return;
    }

    try {
        const response = await fetch(`/api/categories/${id}`, { method: "DELETE" });
        if (!response.ok) {
            categoryStatus.textContent = await getErrorMessage(response, "Could not delete category");
            return;
        }

        if (editingCategoryId === id) {
            resetCategoryForm();
        }
        const loaded = await loadCategories();
        categoryStatus.textContent = loaded ? "Category deleted." : "Category deleted, but the list could not be refreshed.";
    } catch (error) {
        categoryStatus.textContent = `Request error: ${error.message}`;
    }
}

function getName(category) {
    return category.name ?? category.Name ?? "";
}

async function getErrorMessage(response, message) {
    const body = await response.text();
    return `${message}: HTTP ${response.status}${body ? `\n${body}` : ""}`;
}

function formatDate(value) {
    if (!value) {
        return "—";
    }
    const date = new Date(value);
    return Number.isNaN(date.getTime()) ? "—" : date.toLocaleString("uk-UA");
}

loadCategories();
