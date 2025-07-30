let options = [];
let questionType = 'SingleChoice';

function initQuizForm(initialOptions, initialType) {
    options = initialOptions;
    questionType = initialType;
    renderOptions();

    const typeSelect = document.querySelector('#QuestionType');
    if (typeSelect) {
        typeSelect.addEventListener('change', function () {
            questionType = this.value;
            renderOptions();
        });
    }
}

function renderOptions() {
    const container = document.getElementById('option-list');
    container.innerHTML = "";

    options.forEach((opt, index) => {
        const div = document.createElement('div');
        div.className = "border rounded p-3 mb-3 bg-light";

        div.innerHTML = `
            <input type="hidden" name="Options[${index}].OptionId" value="${opt.optionId || 0}" />
            <div class="mb-2">
                <label class="form-label">Option Text</label>
                <input name="Options[${index}].OptionText" class="form-control border border-secondary" value="${opt.optionText || ''}" />
            </div>
            <div class="form-check">
                <input type="${questionType === 'SingleChoice' ? 'radio' : 'checkbox'}"
                       name="answer-selection"
                       class="form-check-input"
                       ${opt.isCorrect ? 'checked' : ''}
                       onchange="setCorrect(${index}, this.checked)" />
                <label class="form-check-label">Correct Answer</label>
                <input type="hidden" name="Options[${index}].IsCorrect" value="${opt.isCorrect}" />
            </div>
            <button type="button" class="btn btn-outline-danger btn-sm mt-2" onclick="removeOption(${index})">Remove</button>
        `;
        container.appendChild(div);
    });
}

function setCorrect(index, checked) {
    if (questionType === 'SingleChoice') {
        options.forEach((opt, i) => opt.isCorrect = (i === index));
    } else {
        options[index].isCorrect = checked;
    }

    // ✅ Cập nhật giá trị của hidden input để model binding đúng
    const hiddenInputs = document.querySelectorAll(`input[name="Options[${index}].IsCorrect"]`);
    hiddenInputs.forEach(input => {
        input.value = options[index].isCorrect;
    });

    if (questionType === 'SingleChoice') {
        renderOptions(); // cần re-render để uncheck các radio khác
    }
}


function addOption() {
    options.push({ optionId: 0, optionText: '', isCorrect: false });
    renderOptions();
}

function removeOption(index) {
    options.splice(index, 1);
    renderOptions();
}
