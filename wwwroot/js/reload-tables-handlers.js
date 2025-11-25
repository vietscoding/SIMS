// Function to reset table and reload data
function resetAndLoad() {
    $("tbody").empty();
    sttCounter = 0;
    currentPage = 0;
    totalPages = 1; // Reset to trigger load
    loadMoreCourses();
}
