// Main JavaScript file for EduLearn Platform

// DOM Content Loaded
document.addEventListener('DOMContentLoaded', function() {
    // Initialize all components
    initSmoothScrolling();
    initFadeInAnimations();
    initCourseFilters();
    initQuizFunctionality();
    initVideoPlayer();
    initProgressBars();
    initFormValidation();
    initModals();
    initSearchFunctionality();
    initDashboardCharts();
});

// Smooth Scrolling for anchor links
function initSmoothScrolling() {
    const links = document.querySelectorAll('a[href^="#"]');
    links.forEach(link => {
        link.addEventListener('click', function(e) {
            e.preventDefault();
            const targetId = this.getAttribute('href');
            const targetElement = document.querySelector(targetId);
            if (targetElement) {
                targetElement.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });
}

// Fade in animations on scroll
function initFadeInAnimations() {
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('visible');
            }
        });
    }, observerOptions);

    // Add fade-in class to elements that should animate
    const animateElements = document.querySelectorAll('.course-card, .testimonial-card, .dashboard-card, .stat-item');
    animateElements.forEach(el => {
        el.classList.add('fade-in');
        observer.observe(el);
    });
}

// Course filtering functionality
function initCourseFilters() {
    const filterButtons = document.querySelectorAll('.filter-btn');
    const courseCards = document.querySelectorAll('.course-card');
    const searchInput = document.getElementById('courseSearch');

    // Filter by category
    filterButtons.forEach(button => {
        button.addEventListener('click', function() {
            const filter = this.getAttribute('data-filter');
            
            // Update active button
            filterButtons.forEach(btn => btn.classList.remove('active'));
            this.classList.add('active');
            
            // Filter courses
            courseCards.forEach(card => {
                const category = card.getAttribute('data-category');
                if (filter === 'all' || category === filter) {
                    card.style.display = 'block';
                    card.classList.add('fade-in');
                } else {
                    card.style.display = 'none';
                }
            });
        });
    });

    // Search functionality
    if (searchInput) {
        searchInput.addEventListener('input', function() {
            const searchTerm = this.value.toLowerCase();
            
            courseCards.forEach(card => {
                const title = card.querySelector('.course-title').textContent.toLowerCase();
                const description = card.querySelector('.course-description').textContent.toLowerCase();
                const instructor = card.querySelector('.instructor-name').textContent.toLowerCase();
                
                if (title.includes(searchTerm) || description.includes(searchTerm) || instructor.includes(searchTerm)) {
                    card.style.display = 'block';
                } else {
                    card.style.display = 'none';
                }
            });
        });
    }
}

// Quiz functionality
function initQuizFunctionality() {
    const quizOptions = document.querySelectorAll('.quiz-option');
    const nextButton = document.getElementById('nextQuestion');
    const prevButton = document.getElementById('prevQuestion');
    const submitButton = document.getElementById('submitQuiz');
    const quizForm = document.getElementById('quizForm');

    let currentQuestion = 0;
    const questions = document.querySelectorAll('.quiz-question');
    const totalQuestions = questions.length;
    let userAnswers = {};

    // Show first question
    if (questions.length > 0) {
        showQuestion(0);
    }

    // Option selection
    quizOptions.forEach(option => {
        option.addEventListener('click', function() {
            const questionId = this.closest('.quiz-question').id;
            const optionValue = this.getAttribute('data-value');
            
            // Remove previous selection for this question
            const questionOptions = this.closest('.quiz-question').querySelectorAll('.quiz-option');
            questionOptions.forEach(opt => opt.classList.remove('selected'));
            
            // Select current option
            this.classList.add('selected');
            userAnswers[questionId] = optionValue;
            
            // Update progress
            updateQuizProgress();
        });
    });

    // Next question
    if (nextButton) {
        nextButton.addEventListener('click', function() {
            if (currentQuestion < totalQuestions - 1) {
                currentQuestion++;
                showQuestion(currentQuestion);
            }
        });
    }

    // Previous question
    if (prevButton) {
        prevButton.addEventListener('click', function() {
            if (currentQuestion > 0) {
                currentQuestion--;
                showQuestion(currentQuestion);
            }
        });
    }

    // Submit quiz
    if (submitButton) {
        submitButton.addEventListener('click', function() {
            if (Object.keys(userAnswers).length === totalQuestions) {
                submitQuiz();
            } else {
                alert('Please answer all questions before submitting.');
            }
        });
    }

    function showQuestion(index) {
        questions.forEach((question, i) => {
            question.style.display = i === index ? 'block' : 'none';
        });
        
        // Update navigation buttons
        if (prevButton) {
            prevButton.style.display = index === 0 ? 'none' : 'inline-block';
        }
        if (nextButton) {
            nextButton.style.display = index === totalQuestions - 1 ? 'none' : 'inline-block';
        }
        if (submitButton) {
            submitButton.style.display = index === totalQuestions - 1 ? 'inline-block' : 'none';
        }
        
        // Update progress
        updateQuizProgress();
    }

    function updateQuizProgress() {
        const progressBar = document.getElementById('quizProgress');
        if (progressBar) {
            const answered = Object.keys(userAnswers).length;
            const progress = (answered / totalQuestions) * 100;
            progressBar.style.width = progress + '%';
            progressBar.setAttribute('aria-valuenow', progress);
        }
    }

    function submitQuiz() {
        // Simulate quiz submission
        const score = calculateScore();
        showQuizResults(score);
    }

    function calculateScore() {
        // This would normally compare with correct answers
        // For demo purposes, return a random score
        return Math.floor(Math.random() * 40) + 60; // 60-100%
    }

    function showQuizResults(score) {
        const resultsModal = document.getElementById('quizResultsModal');
        const scoreElement = document.getElementById('quizScore');
        
        if (scoreElement) {
            scoreElement.textContent = score + '%';
        }
        
        if (resultsModal) {
            const modal = new bootstrap.Modal(resultsModal);
            modal.show();
        }
    }
}

// Video player functionality
function initVideoPlayer() {
    const video = document.getElementById('lessonVideo');
    const playButton = document.getElementById('playButton');
    const progressBar = document.getElementById('videoProgress');
    const timeDisplay = document.getElementById('timeDisplay');
    const volumeSlider = document.getElementById('volumeSlider');

    if (video && playButton) {
        playButton.addEventListener('click', function() {
            if (video.paused) {
                video.play();
                this.innerHTML = '<i class="fas fa-pause"></i>';
            } else {
                video.pause();
                this.innerHTML = '<i class="fas fa-play"></i>';
            }
        });
    }

    if (video && progressBar) {
        video.addEventListener('timeupdate', function() {
            const progress = (video.currentTime / video.duration) * 100;
            progressBar.style.width = progress + '%';
            
            if (timeDisplay) {
                timeDisplay.textContent = formatTime(video.currentTime) + ' / ' + formatTime(video.duration);
            }
        });

        progressBar.addEventListener('click', function(e) {
            const rect = this.getBoundingClientRect();
            const clickX = e.clientX - rect.left;
            const width = rect.width;
            const newTime = (clickX / width) * video.duration;
            video.currentTime = newTime;
        });
    }

    if (video && volumeSlider) {
        volumeSlider.addEventListener('input', function() {
            video.volume = this.value / 100;
        });
    }
}

// Progress bars animation
function initProgressBars() {
    const progressBars = document.querySelectorAll('.progress-bar');
    
    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const progressBar = entry.target;
                const width = progressBar.getAttribute('data-width') || progressBar.style.width;
                progressBar.style.width = '0%';
                
                setTimeout(() => {
                    progressBar.style.width = width;
                }, 100);
            }
        });
    });

    progressBars.forEach(bar => {
        observer.observe(bar);
    });
}

// Form validation
function initFormValidation() {
    const forms = document.querySelectorAll('.needs-validation');
    
    forms.forEach(form => {
        form.addEventListener('submit', function(e) {
            if (!form.checkValidity()) {
                e.preventDefault();
                e.stopPropagation();
            }
            form.classList.add('was-validated');
        });
    });

    // Real-time validation
    const inputs = document.querySelectorAll('input, textarea, select');
    inputs.forEach(input => {
        input.addEventListener('blur', function() {
            validateField(this);
        });
        
        input.addEventListener('input', function() {
            if (this.classList.contains('is-invalid')) {
                validateField(this);
            }
        });
    });
}

function validateField(field) {
    const value = field.value.trim();
    const type = field.type;
    const required = field.hasAttribute('required');
    
    let isValid = true;
    let message = '';
    
    if (required && !value) {
        isValid = false;
        message = 'This field is required.';
    } else if (type === 'email' && value) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(value)) {
            isValid = false;
            message = 'Please enter a valid email address.';
        }
    } else if (type === 'password' && value) {
        if (value.length < 8) {
            isValid = false;
            message = 'Password must be at least 8 characters long.';
        }
    }
    
    if (isValid) {
        field.classList.remove('is-invalid');
        field.classList.add('is-valid');
    } else {
        field.classList.remove('is-valid');
        field.classList.add('is-invalid');
    }
    
    // Update feedback message
    const feedback = field.parentNode.querySelector('.invalid-feedback');
    if (feedback) {
        feedback.textContent = message;
    }
}

// Modal functionality
function initModals() {
    const modals = document.querySelectorAll('.modal');
    
    modals.forEach(modal => {
        modal.addEventListener('show.bs.modal', function() {
            // Add any pre-modal logic here
        });
        
        modal.addEventListener('hidden.bs.modal', function() {
            // Add any post-modal logic here
        });
    });
}

// Search functionality
function initSearchFunctionality() {
    const searchInputs = document.querySelectorAll('.search-input');
    
    searchInputs.forEach(input => {
        let searchTimeout;
        
        input.addEventListener('input', function() {
            clearTimeout(searchTimeout);
            searchTimeout = setTimeout(() => {
                performSearch(this.value);
            }, 300);
        });
    });
}

function performSearch(query) {
    // This would typically make an API call
    console.log('Searching for:', query);
    
    // For demo purposes, just log the search
    if (query.length > 2) {
        // Simulate search results
        showSearchResults(query);
    }
}

function showSearchResults(query) {
    // This would display search results
    console.log('Showing results for:', query);
}

// Dashboard charts (using Chart.js if available)
function initDashboardCharts() {
    // Check if Chart.js is loaded
    if (typeof Chart !== 'undefined') {
        // Course completion chart
        const completionCtx = document.getElementById('completionChart');
        if (completionCtx) {
            new Chart(completionCtx, {
                type: 'doughnut',
                data: {
                    labels: ['Completed', 'In Progress', 'Not Started'],
                    datasets: [{
                        data: [65, 25, 10],
                        backgroundColor: ['#10b981', '#f59e0b', '#e5e7eb'],
                        borderWidth: 0
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            position: 'bottom'
                        }
                    }
                }
            });
        }
        
        // Monthly progress chart
        const progressCtx = document.getElementById('progressChart');
        if (progressCtx) {
            new Chart(progressCtx, {
                type: 'line',
                data: {
                    labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
                    datasets: [{
                        label: 'Courses Completed',
                        data: [2, 4, 3, 6, 8, 5],
                        borderColor: '#2563eb',
                        backgroundColor: 'rgba(37, 99, 235, 0.1)',
                        tension: 0.4,
                        fill: true
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
        }
    }
}

// Utility functions
function formatTime(seconds) {
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = Math.floor(seconds % 60);
    return `${minutes}:${remainingSeconds.toString().padStart(2, '0')}`;
}

function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `alert alert-${type} alert-dismissible fade show position-fixed`;
    notification.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
    notification.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    
    document.body.appendChild(notification);
    
    // Auto remove after 5 seconds
    setTimeout(() => {
        if (notification.parentNode) {
            notification.remove();
        }
    }, 5000);
}

function toggleSidebar() {
    const sidebar = document.getElementById('sidebar');
    if (sidebar) {
        sidebar.classList.toggle('show');
    }
}

// Export functions for global access
window.EduLearn = {
    showNotification,
    toggleSidebar,
    formatTime
};
