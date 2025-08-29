// ILLIYEEN - Shopping Cart JavaScript

document.addEventListener('DOMContentLoaded', function() {
    initializeCartFunctionality();
});

function initializeCartFunctionality() {
    // Add to cart buttons
    const addToCartButtons = document.querySelectorAll('.add-to-cart-btn');
    addToCartButtons.forEach(button => {
        button.addEventListener('click', function(e) {
            e.preventDefault();
            const productId = this.getAttribute('data-product-id');
            const quantity = getQuantityForProduct(productId);
            addToCart(productId, quantity);
        });
    });
    
    // Quantity controls
    initializeQuantityControls();
    
    // Cart update on page load
    updateCartDisplay();
}

function getQuantityForProduct(productId) {
    // Check if there's a quantity input for this product
    const quantityInput = document.getElementById('productQuantity');
    if (quantityInput) {
        return parseInt(quantityInput.value) || 1;
    }
    return 1;
}

function addToCart(productId, quantity = 1) {
    if (!productId) {
        showCartMessage('Invalid product', 'error');
        return;
    }
    
    // Show loading state
    const button = document.querySelector(`[data-product-id="${productId}"]`);
    if (button) {
        const originalText = button.innerHTML;
        button.innerHTML = '<i class="fas fa-spinner fa-spin"></i>';
        button.disabled = true;
        
        // Restore button after operation
        const restoreButton = () => {
            button.innerHTML = originalText;
            button.disabled = false;
        };
        
        setTimeout(restoreButton, 2000); // Fallback restore
    }
    
    fetch('/cart/add', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': getAntiForgeryToken()
        },
        body: JSON.stringify({
            productId: parseInt(productId),
            quantity: quantity
        })
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    })
    .then(data => {
        if (data.success) {
            showCartMessage('Product added to cart!', 'success');
            updateCartCounter(data.cartCount);
            
            // Add visual feedback
            if (button) {
                button.innerHTML = '<i class="fas fa-check me-1"></i>Added!';
                button.classList.remove('btn-outline-gold');
                button.classList.add('btn-success');
                
                setTimeout(() => {
                    button.innerHTML = '<i class="fas fa-shopping-cart me-1"></i>Add to Cart';
                    button.classList.remove('btn-success');
                    button.classList.add('btn-outline-gold');
                    button.disabled = false;
                }, 1500);
            }
        } else {
            throw new Error(data.message || 'Failed to add product to cart');
        }
    })
    .catch(error => {
        console.error('Error adding to cart:', error);
        showCartMessage(error.message || 'Failed to add product to cart', 'error');
        if (button) {
            button.innerHTML = '<i class="fas fa-exclamation-triangle me-1"></i>Error';
            button.classList.add('btn-danger');
            setTimeout(() => {
                button.innerHTML = '<i class="fas fa-shopping-cart me-1"></i>Add to Cart';
                button.classList.remove('btn-danger');
                button.disabled = false;
            }, 2000);
        }
    });
}

function updateCartQuantity(itemId, quantity) {
    if (quantity < 1) {
        removeFromCart(itemId);
        return;
    }
    
    fetch('/cart/update', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': getAntiForgeryToken()
        },
        body: JSON.stringify({
            id: parseInt(itemId),
            quantity: parseInt(quantity)
        })
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    })
    .then(data => {
        if (data.success) {
            updateCartDisplay();
            showCartMessage('Cart updated successfully', 'success');
        } else {
            throw new Error(data.message || 'Failed to update cart');
        }
    })
    .catch(error => {
        console.error('Error updating cart:', error);
        showCartMessage(error.message || 'Failed to update cart', 'error');
        // Revert quantity change
        location.reload();
    });
}

function removeFromCart(itemId) {
    if (!confirm('Are you sure you want to remove this item from your cart?')) {
        return;
    }
    
    fetch('/cart/remove', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': getAntiForgeryToken()
        },
        body: JSON.stringify({
            id: parseInt(itemId)
        })
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json();
    })
    .then(data => {
        if (data.success) {
            // Remove item from DOM with animation
            const cartItem = document.querySelector(`[data-item-id="${itemId}"]`);
            if (cartItem) {
                cartItem.style.transition = 'all 0.3s ease';
                cartItem.style.opacity = '0';
                cartItem.style.transform = 'translateX(-100%)';
                setTimeout(() => {
                    cartItem.remove();
                    updateCartDisplay();
                }, 300);
            }
            showCartMessage('Item removed from cart', 'info');
        } else {
            throw new Error(data.message || 'Failed to remove item');
        }
    })
    .catch(error => {
        console.error('Error removing from cart:', error);
        showCartMessage(error.message || 'Failed to remove item', 'error');
    });
}

function initializeQuantityControls() {
    // Quantity increment/decrement buttons
    document.addEventListener('click', function(e) {
        if (e.target.matches('.quantity-btn')) {
            e.preventDefault();
            const button = e.target;
            const inputGroup = button.closest('.input-group');
            const input = inputGroup.querySelector('.quantity-input');
            const currentValue = parseInt(input.value) || 1;
            const isIncrement = button.textContent.trim() === '+';
            const newValue = isIncrement ? currentValue + 1 : Math.max(1, currentValue - 1);
            
            input.value = newValue;
            
            // Update cart if this is a cart page
            const cartItem = button.closest('[data-item-id]');
            if (cartItem) {
                const itemId = cartItem.getAttribute('data-item-id');
                updateCartQuantity(itemId, newValue);
            }
        }
    });
    
    // Quantity input direct change
    document.addEventListener('change', function(e) {
        if (e.target.matches('.quantity-input')) {
            const input = e.target;
            const value = parseInt(input.value) || 1;
            const min = parseInt(input.getAttribute('min')) || 1;
            const max = parseInt(input.getAttribute('max')) || 999;
            
            // Validate value
            const validValue = Math.max(min, Math.min(max, value));
            input.value = validValue;
            
            // Update cart if this is a cart page
            const cartItem = input.closest('[data-item-id]');
            if (cartItem) {
                const itemId = cartItem.getAttribute('data-item-id');
                updateCartQuantity(itemId, validValue);
            }
        }
    });
}

function updateCartDisplay() {
    // Update cart totals and counts
    updateCartTotals();
    updateCartCounter();
}

function updateCartTotals() {
    let subtotal = 0;
    let totalItems = 0;
    
    // Calculate totals from cart items
    const cartItems = document.querySelectorAll('[data-item-id]');
    cartItems.forEach(item => {
        const quantityInput = item.querySelector('.quantity-input');
        const priceElement = item.querySelector('.item-price, [data-price]');
        
        if (quantityInput && priceElement) {
            const quantity = parseInt(quantityInput.value) || 0;
            const price = parseFloat(priceElement.getAttribute('data-price')) || 
                         parseFloat(priceElement.textContent.replace(/[^\d.]/g, '')) || 0;
            
            const itemTotal = price * quantity;
            subtotal += itemTotal;
            totalItems += quantity;
            
            // Update item total display
            const itemTotalElement = item.querySelector('.item-total');
            if (itemTotalElement) {
                itemTotalElement.textContent = `৳${itemTotal.toLocaleString()}`;
            }
        }
    });
    
    // Update subtotal display
    const subtotalElement = document.getElementById('cart-subtotal');
    if (subtotalElement) {
        subtotalElement.textContent = `৳${subtotal.toLocaleString()}`;
    }
    
    // Calculate shipping
    const shippingCost = subtotal >= 5000 ? 0 : 100;
    const total = subtotal + shippingCost;
    
    // Update total display
    const totalElement = document.getElementById('cart-total');
    if (totalElement) {
        totalElement.textContent = `৳${total.toLocaleString()}`;
    }
    
    // Update shipping message
    updateShippingMessage(subtotal);
}

function updateShippingMessage(subtotal) {
    const shippingElements = document.querySelectorAll('.shipping-message');
    shippingElements.forEach(element => {
        if (subtotal >= 5000) {
            element.innerHTML = '<span class="text-success">Free</span>';
        } else {
            element.innerHTML = '৳100';
        }
    });
    
    // Update free shipping notification
    const freeShippingAlert = document.querySelector('.free-shipping-alert');
    if (freeShippingAlert) {
        const remaining = 5000 - subtotal;
        if (remaining > 0) {
            freeShippingAlert.innerHTML = `
                <i class="fas fa-info-circle me-1"></i>
                Add ৳${remaining.toLocaleString()} more for free shipping!
            `;
            freeShippingAlert.style.display = 'block';
        } else {
            freeShippingAlert.style.display = 'none';
        }
    }
}

function updateCartCounter(count) {
    if (count !== undefined) {
        const cartBadge = document.getElementById('cart-count');
        if (cartBadge) {
            cartBadge.textContent = count;
            if (count > 0) {
                cartBadge.style.display = 'inline-block';
                // Add pulse animation
                cartBadge.classList.add('pulse');
                setTimeout(() => cartBadge.classList.remove('pulse'), 1000);
            } else {
                cartBadge.style.display = 'none';
            }
        }
    } else {
        // Fetch current cart count
        if (window.IlliyeenApp && window.IlliyeenApp.updateCartCounter) {
            window.IlliyeenApp.updateCartCounter();
        }
    }
}

function showCartMessage(message, type = 'info') {
    // Use the global notification system if available
    if (window.IlliyeenApp && window.IlliyeenApp.showNotification) {
        window.IlliyeenApp.showNotification(message, type);
        return;
    }
    
    // Fallback notification
    const alertClass = type === 'error' ? 'danger' : type;
    const alert = document.createElement('div');
    alert.className = `alert alert-${alertClass} alert-dismissible fade show position-fixed`;
    alert.style.cssText = 'top: 20px; right: 20px; z-index: 1060; min-width: 300px;';
    alert.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    
    document.body.appendChild(alert);
    
    // Auto dismiss after 3 seconds
    setTimeout(() => {
        if (alert.parentNode) {
            alert.remove();
        }
    }, 3000);
}

function getAntiForgeryToken() {
    const token = document.querySelector('input[name="__RequestVerificationToken"]');
    return token ? token.value : '';
}

// Mini cart functionality (if implemented)
function toggleMiniCart() {
    const miniCart = document.querySelector('.mini-cart');
    if (miniCart) {
        miniCart.classList.toggle('show');
    }
}

function loadMiniCartContent() {
    fetch('/cart/mini', {
        method: 'GET',
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    })
    .then(response => response.text())
    .then(html => {
        const miniCartContent = document.querySelector('.mini-cart-content');
        if (miniCartContent) {
            miniCartContent.innerHTML = html;
        }
    })
    .catch(error => {
        console.error('Error loading mini cart:', error);
    });
}

// Wishlist functionality (if implemented)
function addToWishlist(productId) {
    fetch('/wishlist/add', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': getAntiForgeryToken()
        },
        body: JSON.stringify({ productId: parseInt(productId) })
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            showCartMessage('Added to wishlist!', 'success');
            updateWishlistButton(productId, true);
        } else {
            throw new Error(data.message || 'Failed to add to wishlist');
        }
    })
    .catch(error => {
        console.error('Error adding to wishlist:', error);
        showCartMessage(error.message || 'Failed to add to wishlist', 'error');
    });
}

function removeFromWishlist(productId) {
    fetch('/wishlist/remove', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': getAntiForgeryToken()
        },
        body: JSON.stringify({ productId: parseInt(productId) })
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            showCartMessage('Removed from wishlist', 'info');
            updateWishlistButton(productId, false);
        } else {
            throw new Error(data.message || 'Failed to remove from wishlist');
        }
    })
    .catch(error => {
        console.error('Error removing from wishlist:', error);
        showCartMessage(error.message || 'Failed to remove from wishlist', 'error');
    });
}

function updateWishlistButton(productId, isInWishlist) {
    const wishlistButton = document.querySelector(`[data-wishlist-product-id="${productId}"]`);
    if (wishlistButton) {
        if (isInWishlist) {
            wishlistButton.innerHTML = '<i class="fas fa-heart"></i>';
            wishlistButton.classList.add('text-danger');
            wishlistButton.setAttribute('onclick', `removeFromWishlist(${productId})`);
        } else {
            wishlistButton.innerHTML = '<i class="far fa-heart"></i>';
            wishlistButton.classList.remove('text-danger');
            wishlistButton.setAttribute('onclick', `addToWishlist(${productId})`);
        }
    }
}

// Quick view functionality (if implemented)
function quickView(productId) {
    fetch(`/product/quickview/${productId}`, {
        method: 'GET',
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        }
    })
    .then(response => response.text())
    .then(html => {
        // Create and show modal with product details
        const modal = document.createElement('div');
        modal.className = 'modal fade';
        modal.innerHTML = html;
        document.body.appendChild(modal);
        
        const bsModal = new bootstrap.Modal(modal);
        bsModal.show();
        
        // Remove modal from DOM when hidden
        modal.addEventListener('hidden.bs.modal', function() {
            modal.remove();
        });
    })
    .catch(error => {
        console.error('Error loading quick view:', error);
        showCartMessage('Failed to load product details', 'error');
    });
}

// Export functions for global use
window.addToCart = addToCart;
window.updateCartQuantity = updateCartQuantity;
window.removeFromCart = removeFromCart;
window.addToWishlist = addToWishlist;
window.removeFromWishlist = removeFromWishlist;
window.quickView = quickView;
