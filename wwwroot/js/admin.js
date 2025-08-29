// ILLIYEEN Admin Panel JavaScript

document.addEventListener('DOMContentLoaded', function() {
    initializeAdminPanel();
});

function initializeAdminPanel() {
    initializeSidebar();
    initializeDataTables();
    initializeCharts();
    initializeFormValidation();
    initializeFileUploads();
    initializeNotifications();
    initializeBulkActions();
    
    // Auto-refresh data every 5 minutes
    setInterval(refreshDashboardData, 300000);
}

// Sidebar functionality
function initializeSidebar() {
    const sidebarToggle = document.getElementById('sidebarToggle');
    const sidebar = document.querySelector('.admin-sidebar');
    
    if (sidebarToggle && sidebar) {
        sidebarToggle.addEventListener('click', function() {
            sidebar.classList.toggle('show');
        });
        
        // Close sidebar when clicking outside on mobile
        document.addEventListener('click', function(e) {
            if (window.innerWidth <= 768 && 
                !sidebar.contains(e.target) && 
                !sidebarToggle.contains(e.target) &&
                sidebar.classList.contains('show')) {
                sidebar.classList.remove('show');
            }
        });
    }
    
    // Active link highlighting
    const currentPath = window.location.pathname;
    const navLinks = document.querySelectorAll('.admin-sidebar .nav-link');
    navLinks.forEach(link => {
        if (link.getAttribute('href') === currentPath) {
            link.classList.add('active');
        }
    });
}

// Data tables enhancement
function initializeDataTables() {
    const tables = document.querySelectorAll('.data-table, #productsTable, #ordersTable, #usersTable');
    tables.forEach(table => {
        enhanceTable(table);
    });
}

function enhanceTable(table) {
    // Add sorting functionality
    const headers = table.querySelectorAll('thead th[data-sortable]');
    headers.forEach(header => {
        header.style.cursor = 'pointer';
        header.addEventListener('click', function() {
            sortTable(table, this);
        });
        
        // Add sort icon
        if (!header.querySelector('.sort-icon')) {
            header.innerHTML += ' <i class="fas fa-sort sort-icon text-muted"></i>';
        }
    });
    
    // Add search functionality
    addTableSearch(table);
    
    // Add row selection
    addRowSelection(table);
}

function sortTable(table, header) {
    const tbody = table.querySelector('tbody');
    const rows = Array.from(tbody.querySelectorAll('tr'));
    const columnIndex = Array.from(header.parentNode.children).indexOf(header);
    const currentSort = header.getAttribute('data-sort') || 'asc';
    const newSort = currentSort === 'asc' ? 'desc' : 'asc';
    
    // Update sort icons
    table.querySelectorAll('.sort-icon').forEach(icon => {
        icon.className = 'fas fa-sort sort-icon text-muted';
    });
    
    const sortIcon = header.querySelector('.sort-icon');
    sortIcon.className = `fas fa-sort-${newSort === 'asc' ? 'up' : 'down'} sort-icon text-primary`;
    header.setAttribute('data-sort', newSort);
    
    // Sort rows
    rows.sort((a, b) => {
        const aVal = a.children[columnIndex].textContent.trim();
        const bVal = b.children[columnIndex].textContent.trim();
        
        // Check if values are numeric
        const aNum = parseFloat(aVal.replace(/[^\d.-]/g, ''));
        const bNum = parseFloat(bVal.replace(/[^\d.-]/g, ''));
        
        if (!isNaN(aNum) && !isNaN(bNum)) {
            return newSort === 'asc' ? aNum - bNum : bNum - aNum;
        } else {
            return newSort === 'asc' ? 
                aVal.localeCompare(bVal) : 
                bVal.localeCompare(aVal);
        }
    });
    
    // Reorder DOM
    rows.forEach(row => tbody.appendChild(row));
}

function addTableSearch(table) {
    const existingSearch = table.parentNode.querySelector('.table-search');
    if (existingSearch) return;
    
    const searchContainer = document.createElement('div');
    searchContainer.className = 'table-search mb-3';
    searchContainer.innerHTML = `
        <div class="input-group">
            <span class="input-group-text"><i class="fas fa-search"></i></span>
            <input type="text" class="form-control" placeholder="Search table...">
        </div>
    `;
    
    table.parentNode.insertBefore(searchContainer, table);
    
    const searchInput = searchContainer.querySelector('input');
    searchInput.addEventListener('input', function() {
        filterTable(table, this.value);
    });
}

function filterTable(table, searchTerm) {
    const tbody = table.querySelector('tbody');
    const rows = tbody.querySelectorAll('tr');
    
    rows.forEach(row => {
        const text = row.textContent.toLowerCase();
        if (text.includes(searchTerm.toLowerCase())) {
            row.style.display = '';
        } else {
            row.style.display = 'none';
        }
    });
}

function addRowSelection(table) {
    const thead = table.querySelector('thead tr');
    const tbody = table.querySelector('tbody');
    
    if (!thead || !tbody) return;
    
    // Add select all checkbox to header
    const selectAllTh = document.createElement('th');
    selectAllTh.innerHTML = '<input type="checkbox" class="form-check-input select-all">';
    thead.insertBefore(selectAllTh, thead.firstChild);
    
    // Add select checkbox to each row
    const rows = tbody.querySelectorAll('tr');
    rows.forEach(row => {
        const selectTd = document.createElement('td');
        selectTd.innerHTML = '<input type="checkbox" class="form-check-input row-select">';
        row.insertBefore(selectTd, row.firstChild);
    });
    
    // Handle select all
    const selectAllCheckbox = thead.querySelector('.select-all');
    selectAllCheckbox.addEventListener('change', function() {
        const rowCheckboxes = tbody.querySelectorAll('.row-select');
        rowCheckboxes.forEach(checkbox => {
            checkbox.checked = this.checked;
        });
        updateBulkActions();
    });
    
    // Handle individual row selection
    tbody.addEventListener('change', function(e) {
        if (e.target.classList.contains('row-select')) {
            updateBulkActions();
            
            // Update select all checkbox
            const totalRows = tbody.querySelectorAll('.row-select').length;
            const selectedRows = tbody.querySelectorAll('.row-select:checked').length;
            selectAllCheckbox.checked = selectedRows === totalRows;
            selectAllCheckbox.indeterminate = selectedRows > 0 && selectedRows < totalRows;
        }
    });
}

function updateBulkActions() {
    const selectedRows = document.querySelectorAll('.row-select:checked').length;
    const bulkActions = document.querySelector('.bulk-actions');
    
    if (bulkActions) {
        if (selectedRows > 0) {
            bulkActions.style.display = 'block';
            bulkActions.querySelector('.selected-count').textContent = selectedRows;
        } else {
            bulkActions.style.display = 'none';
        }
    }
}

// Charts initialization
function initializeCharts() {
    // Sales chart
    const salesChartCanvas = document.getElementById('salesChart');
    if (salesChartCanvas && typeof Chart !== 'undefined') {
        createSalesChart(salesChartCanvas);
    }
    
    // Orders chart
    const ordersChartCanvas = document.getElementById('ordersChart');
    if (ordersChartCanvas && typeof Chart !== 'undefined') {
        createOrdersChart(ordersChartCanvas);
    }
}

function createSalesChart(canvas) {
    const ctx = canvas.getContext('2d');
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
            datasets: [{
                label: 'Sales',
                data: [12000, 19000, 15000, 25000, 22000, 30000],
                borderColor: '#d4af37',
                backgroundColor: 'rgba(212, 175, 55, 0.1)',
                tension: 0.4
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    display: false
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: function(value) {
                            return '৳' + value.toLocaleString();
                        }
                    }
                }
            }
        }
    });
}

function createOrdersChart(canvas) {
    const ctx = canvas.getContext('2d');
    new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['Pending', 'Processing', 'Shipped', 'Delivered'],
            datasets: [{
                data: [15, 25, 30, 130],
                backgroundColor: [
                    '#ffc107',
                    '#17a2b8',
                    '#6c757d',
                    '#28a745'
                ]
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'bottom'
                }
            }
        }
    });
}

// Form validation
function initializeFormValidation() {
    const forms = document.querySelectorAll('.admin-form');
    forms.forEach(form => {
        form.addEventListener('submit', function(e) {
            if (!validateAdminForm(form)) {
                e.preventDefault();
            }
        });
    });
}

function validateAdminForm(form) {
    let isValid = true;
    const requiredFields = form.querySelectorAll('[required]');
    
    requiredFields.forEach(field => {
        if (!field.value.trim()) {
            showFieldError(field, 'This field is required');
            isValid = false;
        } else {
            clearFieldError(field);
        }
    });
    
    // Custom validation rules
    const emailFields = form.querySelectorAll('input[type="email"]');
    emailFields.forEach(field => {
        if (field.value && !isValidEmail(field.value)) {
            showFieldError(field, 'Please enter a valid email address');
            isValid = false;
        }
    });
    
    const numberFields = form.querySelectorAll('input[type="number"]');
    numberFields.forEach(field => {
        if (field.value && isNaN(field.value)) {
            showFieldError(field, 'Please enter a valid number');
            isValid = false;
        }
    });
    
    return isValid;
}

function showFieldError(field, message) {
    field.classList.add('is-invalid');
    
    let errorDiv = field.parentNode.querySelector('.invalid-feedback');
    if (!errorDiv) {
        errorDiv = document.createElement('div');
        errorDiv.className = 'invalid-feedback';
        field.parentNode.appendChild(errorDiv);
    }
    errorDiv.textContent = message;
}

function clearFieldError(field) {
    field.classList.remove('is-invalid');
    const errorDiv = field.parentNode.querySelector('.invalid-feedback');
    if (errorDiv) {
        errorDiv.remove();
    }
}

function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

// File uploads
function initializeFileUploads() {
    const fileInputs = document.querySelectorAll('input[type="file"]');
    fileInputs.forEach(input => {
        enhanceFileInput(input);
    });
}

function enhanceFileInput(input) {
    const wrapper = document.createElement('div');
    wrapper.className = 'file-upload-wrapper';
    
    input.parentNode.insertBefore(wrapper, input);
    wrapper.appendChild(input);
    
    const label = document.createElement('label');
    label.className = 'file-upload-label btn btn-outline-gold';
    label.setAttribute('for', input.id);
    label.innerHTML = '<i class="fas fa-cloud-upload-alt me-2"></i>Choose File';
    wrapper.appendChild(label);
    
    const preview = document.createElement('div');
    preview.className = 'file-preview mt-2';
    wrapper.appendChild(preview);
    
    input.addEventListener('change', function() {
        const file = this.files[0];
        if (file) {
            label.textContent = file.name;
            
            // Show image preview if it's an image
            if (file.type.startsWith('image/')) {
                const reader = new FileReader();
                reader.onload = function(e) {
                    preview.innerHTML = `<img src="${e.target.result}" class="img-thumbnail" style="max-width: 200px;">`;
                };
                reader.readAsDataURL(file);
            } else {
                preview.innerHTML = `<i class="fas fa-file me-2"></i>${file.name}`;
            }
        }
    });
}

// Notifications
function initializeNotifications() {
    // Check for new notifications periodically
    if (document.querySelector('.notifications-container')) {
        setInterval(checkNewNotifications, 60000); // Every minute
    }
}

function checkNewNotifications() {
    fetch('/admin/notifications/check', {
        method: 'GET',
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    })
    .then(response => response.json())
    .then(data => {
        if (data.hasNew) {
            updateNotificationBadge(data.count);
        }
    })
    .catch(error => {
        console.error('Error checking notifications:', error);
    });
}

function updateNotificationBadge(count) {
    const badge = document.querySelector('.notification-badge');
    if (badge) {
        badge.textContent = count;
        badge.style.display = count > 0 ? 'inline-block' : 'none';
    }
}

// Bulk actions
function initializeBulkActions() {
    const bulkActionForm = document.querySelector('.bulk-action-form');
    if (bulkActionForm) {
        bulkActionForm.addEventListener('submit', function(e) {
            e.preventDefault();
            executeBulkAction();
        });
    }
}

function executeBulkAction() {
    const selectedRows = document.querySelectorAll('.row-select:checked');
    const action = document.querySelector('.bulk-action-select').value;
    
    if (selectedRows.length === 0) {
        showAdminNotification('Please select at least one item', 'warning');
        return;
    }
    
    if (!action) {
        showAdminNotification('Please select an action', 'warning');
        return;
    }
    
    if (confirm(`Are you sure you want to ${action} ${selectedRows.length} items?`)) {
        const ids = Array.from(selectedRows).map(row => 
            row.closest('tr').getAttribute('data-id')
        ).filter(id => id);
        
        fetch('/admin/bulk-action', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': getAntiForgeryToken()
            },
            body: JSON.stringify({
                action: action,
                ids: ids
            })
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                showAdminNotification(`Successfully ${action}ed ${ids.length} items`, 'success');
                location.reload();
            } else {
                throw new Error(data.message || 'Bulk action failed');
            }
        })
        .catch(error => {
            console.error('Bulk action error:', error);
            showAdminNotification(error.message || 'Bulk action failed', 'error');
        });
    }
}

// Dashboard data refresh
function refreshDashboardData() {
    if (window.location.pathname.includes('/admin/dashboard')) {
        fetch('/admin/dashboard/data', {
            method: 'GET',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
        .then(response => response.json())
        .then(data => {
            updateDashboardStats(data);
        })
        .catch(error => {
            console.error('Error refreshing dashboard data:', error);
        });
    }
}

function updateDashboardStats(data) {
    // Update stat cards
    const statCards = document.querySelectorAll('.stat-card');
    statCards.forEach(card => {
        const metric = card.getAttribute('data-metric');
        if (data[metric] !== undefined) {
            const numberElement = card.querySelector('.stat-number');
            if (numberElement) {
                animateNumber(numberElement, parseInt(numberElement.textContent), data[metric]);
            }
        }
    });
}

function animateNumber(element, start, end) {
    const duration = 1000;
    const startTime = performance.now();
    
    function update(currentTime) {
        const elapsed = currentTime - startTime;
        const progress = Math.min(elapsed / duration, 1);
        
        const current = Math.floor(start + (end - start) * progress);
        element.textContent = current.toLocaleString();
        
        if (progress < 1) {
            requestAnimationFrame(update);
        }
    }
    
    requestAnimationFrame(update);
}

// Admin notifications
function showAdminNotification(message, type = 'info', duration = 5000) {
    const notification = document.createElement('div');
    notification.className = `alert alert-${type} alert-dismissible fade show admin-notification position-fixed`;
    notification.style.cssText = 'top: 20px; right: 20px; z-index: 1060; min-width: 300px;';
    notification.innerHTML = `
        <div class="d-flex align-items-center">
            <i class="fas fa-${getIconForType(type)} me-2"></i>
            <div class="flex-grow-1">${message}</div>
            <button type="button" class="btn-close ms-2" data-bs-dismiss="alert"></button>
        </div>
    `;
    
    document.body.appendChild(notification);
    
    if (duration > 0) {
        setTimeout(() => {
            if (notification.parentNode) {
                notification.remove();
            }
        }, duration);
    }
}

function getIconForType(type) {
    const icons = {
        success: 'check-circle',
        error: 'exclamation-triangle',
        warning: 'exclamation-circle',
        info: 'info-circle'
    };
    return icons[type] || 'info-circle';
}

// Utility functions
function getAntiForgeryToken() {
    const token = document.querySelector('input[name="__RequestVerificationToken"]');
    return token ? token.value : '';
}

function formatCurrency(amount) {
    return new Intl.NumberFormat('bn-BD', {
        style: 'currency',
        currency: 'BDT',
        minimumFractionDigits: 0
    }).format(amount);
}

// Export functions for global use
window.AdminPanel = {
    showNotification: showAdminNotification,
    refreshDashboardData,
    updateDashboardStats,
    validateForm: validateAdminForm,
    formatCurrency
};

// Handle page visibility for auto-refresh
document.addEventListener('visibilitychange', function() {
    if (!document.hidden && window.location.pathname.includes('/admin')) {
        refreshDashboardData();
    }
});
