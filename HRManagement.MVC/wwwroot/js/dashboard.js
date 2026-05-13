(() => {
    const tables = document.querySelectorAll('[data-dashboard-table]');
    tables.forEach((table) => {
        const rows = Array.from(table.querySelectorAll('tbody tr'));
        if (rows.length === 0) {
            const empty = document.createElement('tr');
            const cell = document.createElement('td');
            cell.colSpan = table.querySelectorAll('thead th').length || 1;
            cell.className = 'text-center text-muted py-5';
            cell.textContent = 'No records found.';
            empty.appendChild(cell);
            table.querySelector('tbody')?.appendChild(empty);
        }
    });

    document.querySelectorAll('[data-filter-form]').forEach((form) => {
        form.addEventListener('submit', () => {
            form.querySelectorAll('input, select').forEach((field) => {
                if (!field.value) {
                    field.disabled = true;
                }
            });
        });
    });

    document.querySelector('[data-sidebar-toggle]')?.addEventListener('click', () => {
        if (window.matchMedia('(max-width: 980px)').matches) {
            document.body.classList.toggle('sidebar-open');
            return;
        }
        document.body.classList.toggle('sidebar-collapsed');
    });
})();
