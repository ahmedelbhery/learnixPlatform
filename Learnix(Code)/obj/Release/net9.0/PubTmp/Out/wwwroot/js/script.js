document.addEventListener('DOMContentLoaded', () => {
    // Mobile Menu Toggle
    const hamburger = document.querySelector('.hamburger');
    const navLinks = document.querySelector('.nav-links');
    const navActions = document.querySelector('.nav-actions');

    if (hamburger) {
        hamburger.addEventListener('click', () => {
            navLinks.classList.toggle('active');
            navActions.classList.toggle('active');
            
            // Simple animation for hamburger
            const spans = hamburger.querySelectorAll('span');
            spans[0].style.transform = navLinks.classList.contains('active') ? 'rotate(45deg) translate(5px, 5px)' : 'none';
            spans[1].style.opacity = navLinks.classList.contains('active') ? '0' : '1';
            spans[2].style.transform = navLinks.classList.contains('active') ? 'rotate(-45deg) translate(5px, -5px)' : 'none';
        });
    }

    // Add mobile styles dynamically for the menu if needed
    // (Ideally this should be in CSS, but adding a quick toggle class handler here)
    const style = document.createElement('style');
    style.innerHTML = `
        @media (max-width: 768px) {
            .nav-links.active {
                display: flex;
                flex-direction: column;
                position: absolute;
                top: 70px;
                left: 0;
                width: 100%;
                background: white;
                padding: 20px;
                box-shadow: 0 10px 20px rgba(0,0,0,0.1);
                z-index: 999;
            }
            .nav-actions.active {
                display: flex;
                flex-direction: column;
                position: absolute;
                top: 350px; /* Adjust based on links height */
                left: 0;
                width: 100%;
                background: white;
                padding: 0 20px 20px;
                box-shadow: 0 10px 20px rgba(0,0,0,0.1);
                z-index: 999;
            }
        }
    `;
    document.head.appendChild(style);

    // Smooth Scroll for Anchor Links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth'
                });
                // Close mobile menu if open
                if (navLinks.classList.contains('active')) {
                    navLinks.classList.remove('active');
                    navActions.classList.remove('active');
                }
            }
        });
    });

    // Simple Intersection Observer for Fade-in animations
    const observerOptions = {
        threshold: 0.1
    };

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.opacity = '1';
                entry.target.style.transform = 'translateY(0)';
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);

    document.querySelectorAll('.section, .hero, .card, .course-card').forEach(el => {
        el.style.opacity = '0';
        el.style.transform = 'translateY(20px)';
        el.style.transition = 'opacity 0.6s ease-out, transform 0.6s ease-out';
        observer.observe(el);
    });
});
