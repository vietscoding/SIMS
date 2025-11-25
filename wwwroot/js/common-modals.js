// Đóng add course modal khi user nhấn nút cancel
$('#cancelAddNewCourseBtn').on('click', function (e) {
    if (e.target === this) {
        $('#addCourseModal').hide(); // Đóng modal add course khi nhấn nút Cancel
    }
});

// Đóng update course modal khi user nhấn nút cancel
$('#cancelUpdateCourseBtn').on('click', function (e) {
    if (e.target === this) {
        $('#updateCourseModal').hide(); // Đóng modal update course khi nhấn nút Cancel
    }
});

// Khi user ở trong (các) modal nhấn esc để thoát
$(document).on("keydown", function (e) {
    if (e.key === "Escape") {
        $("#addCourseModal").hide(); // Đóng modal add course khi nhấn esc
        $("#courseDetailsModal").hide(); // Đóng modal course details khi nhấn esc
        $("#filterModal").hide(); // Đóng modal filter khi nhấn esc
        $("#updateCourseModal").hide(); // Đóng modal filter khi nhấn esc
    }
});

// Khi user đang trong modal Add Course nhấn Enter để submit
$(document).on("keydown", function (e) {
    if (e.key === "Enter" && $("#addCourseModal").is(":visible")) {
        $("#addNewCourseBtn").click();
    }
});

// Khi user đang trong modal Filter Modal nhấn Enter để apply
$(document).on("keydown", function (e) {
    if (e.key === "Enter" && $("#applyFilterBtn").is(":visible")) {
        $("#applyFilterBtn").click();
    }
});

// Update course modal open/close
$("#closeUpdateCourseModal").on("click", function () {
    $("#updateCourseModal").hide(); // Đóng update  modal 
});
// Close when clicking outside content | Đóng update modal khi click ngoài nội dung
$("#updateCourseModal").on("click", function (e) {
    if (e.target === this) {
        $(this).hide();
    }
});

// Filter course modal open/close
$("#filterBtn").on("click", function () {
    $("#filterModal").css("display", "flex");
});
$("#closeFilterModal").on("click", function () {
    $("#filterModal").hide(); // Đóng filter modal
});
// Close when clicking outside content | Đóng filter modal khi click ngoài nội dung
$("#filterModal").on("click", function (e) {
    if (e.target === this) {
        $(this).hide();
    }
});


// Xử lý hiện/ẩn Add Course Modal
$("#addCourseBtn").on("click", function () {
    $("#addCourseModal").css("display", "flex");
});
$("#closeAddCourseModal").on("click", function () {
    $("#addCourseModal").hide();
});
$("#cancelAddCourseBtn").on("click", function () {
    $("#addCourseModal").hide();
});
$("#addCourseModal").on("click", function (e) {
    if (e.target === this) {
        $(this).hide();
    }
});
