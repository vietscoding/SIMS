// sidebar.js
(function () {
    const sidebar = document.getElementById('leftSidebar');
    const toggle = document.getElementById('sidebarToggle');
    if (!sidebar || !toggle) return;

    // restore persisted state
    const collapsed = localStorage.getItem('adminSidebarCollapsed') === 'true';
    if (collapsed) sidebar.classList.add('collapsed');

    // reflect aria-expanded
    toggle.setAttribute('aria-expanded', (!collapsed).toString());

    toggle.addEventListener('click', function () {
        const isCollapsed = sidebar.classList.toggle('collapsed'); // true = collapsed
        toggle.setAttribute('aria-expanded', (!isCollapsed).toString());
        localStorage.setItem('adminSidebarCollapsed', isCollapsed);
    });

    // Optional: Close overlay sidebar on small screens when clicking outside
    document.addEventListener('click', (e) => {
        if (window.innerWidth <= 800) {
            if (!sidebar.contains(e.target) && !toggle.contains(e.target)) {
                sidebar.classList.remove('open');
            }
        }
    });
})();

const userInfo = document.getElementById('userInfo');
const userModal = document.getElementById('userModal');
const closeModal = document.querySelector('.close-modal');

if (userInfo && userModal && closeModal) {
    userInfo.addEventListener('click', () => {
        userModal.style.display = 'flex';
        document.getElementById('mainContent').style.filter = 'blur(4px)';
    });

    closeModal.addEventListener('click', () => {
        userModal.style.display = 'none';
        document.getElementById('mainContent').style.filter = 'none';
    });

    window.addEventListener('click', (e) => {
        if (e.target === userModal) {
            userModal.style.display = 'none';
            document.getElementById('mainContent').style.filter = 'none';
        }
    });
}


//const sidebar = document.getElementById('leftSidebar');
//const content = document.getElementById('mainContent');
//const toggleBtn = document.getElementById('sidebarToggle');

//toggleBtn.addEventListener('click', () => {
//    sidebar.classList.toggle('collapsed');
//});
