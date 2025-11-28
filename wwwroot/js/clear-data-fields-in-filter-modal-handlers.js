// Nút Clear xóa hết dữ liệu trong các trường của Filter Modal
$("#clearFilterBtn").on("click", function () {
    $("#filterName").val("");
    $("#filterCode").val("");
    $("#filterFacultyId").val("");
    $("#filterLectureMin").val("");
    $("#filterLectureMax").val("");
    $("#filterPracticalMin").val("");
    $("#filterPracticalMax").val("");
    $("#filterInternshipMin").val("");
    $("#filterInternshipMax").val("");
    $("#filterCapstoneMin").val("");
    $("#filterCapstoneMax").val("");
    $("#filterTotalMin").val("");
    $("#filterTotalMax").val("");
    $("#filterSummary").val("");
    filters = {};
    $("#filterBtn").removeClass("has-active-filters"); // Xóa luôn cả red dot (cái xuất hiện khi nhất nút Apply Filter)
    resetAndLoad(); // Tải lại trang
});
