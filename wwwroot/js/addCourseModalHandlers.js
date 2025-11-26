// Khi người dùng chọn một option từ datalist
$("#facultyInCharge").on("input change", function () {
    const inputVal = $(this).val().trim();
    let selectedFacultyId = "";

    // Duyệt qua tất cả option trong datalist để tìm tên khớp
    $("#facultyInChargeList option").each(function () {
        if ($(this).val() === inputVal) {
            selectedFacultyId = $(this).data("id");
            return false; // break
        }
    });
    $("#facultyId").val(selectedFacultyId || ""); // nếu không tìm thấy thì để trống
});

// Trường hợp người dùng xóa hết hoặc nhập tay không khớp → reset facultyId
$("#facultyInCharge").on("blur", function () {
    const val = $(this).val().trim();
    let found = false;
    $("#facultyInChargeList option").each(function () {
        if ($(this).val() === val) {
            found = true;
            return false;
        }
    });
    if (!found) {
        $("#facultyId").val("");
    }
});

// Update total credits whenever credit fields change (Add modal)
function updateTotalCredits() {
    const lecture = parseFloat($("#lectureCredits").val() || 0) || 0;
    const practical = parseFloat($("#practicalCredits").val() || 0) || 0;
    const internship = parseFloat($("#internshipCredits").val() || 0) || 0;
    const capstone = parseFloat($("#capstoneCredits").val() || 0) || 0;
    const total = lecture + practical + capstone;
    $("#totalCredits").val(total);
}
$("#lectureCredits, #practicalCredits, #internshipCredits, #capstoneCredits").on("input change", updateTotalCredits);


// Add new course handler
$("#addNewCourseBtn").on("click", function () {

    // Gọi hàm cập nhật tổng tín chỉ trước khi gửi
    updateTotalCredits();
    // client-side validation
    const courseName = $("#courseName").val()?.toString().trim() || "";
    const tenHocPhan = $("#tenHocPhan").val()?.toString().trim() || "";
    const courseCode = $("#courseCode").val()?.toString().trim() || "";
    const faculty = $("#facultyInCharge").val()?.toString().trim() || "";
    const facultyId = $("#facultyId").val()?.toString().trim() || "";


    const lecture = parseFloat($("#lectureCredits").val() || 0) || 0;
    const practical = parseFloat($("#practicalCredits").val() || 0) || 0;
    const internship = parseFloat($("#internshipCredits").val() || 0) || 0;
    const capstone = parseFloat($("#capstoneCredits").val() || 0) || 0;
    // const totalCredits = parseFloat($("#totalCredits").val() || 0) || 0;
    const summary = $("#courseSummary").val()?.toString().trim() || "";


    if (!courseName) {
        alert("Course name is required.");
        return;
    }
    if (!/^\d{7}$/.test(courseCode)) {
        alert("Course code is required and must be exactly 7 digits.");
        return;
    }
    if (lecture < 0 || practical < 0 || internship < 0 || capstone < 0) {
        alert("Credit values must be 0 or positive.");
        return;
    }

    const payload = {
        courseName: courseName,
        tenHocPhan: tenHocPhan,
        courseCode: courseCode,
        facultyId: $("#facultyId").val() || null,
        lectureCredits: lecture,
        practicalCredits: practical,
        internshipCredits: internship,
        capstoneCredits: capstone,
        courseSummary: summary
    };

    // disable button while submitting
    $("#addNewCourseBtn").prop("disabled", true).text("Adding...");

    $.ajax({
        url: "/Admin/AddCourse",
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify(payload),
        success: function (res) {
            alert(res?.message || "Course added successfully.");
            $("#addCourseModal").hide();
            // reset form
            $("#courseName, #tenHocPhan, #courseCode, #facultyInCharge, #lectureCredits, #practicalCredits, #internshipCredits, #capstoneCredits, #courseSummary").val("");
            $("#totalCredits").val("");
            // refresh list to show newly added course: clear and reload
            resetAndLoad();
        },
        error: function (xhr) {
            let message = "Failed to add course.";
            try {
                const json = JSON.parse(xhr.responseText || "{}");
                if (json?.message) message = json.message;
            } catch (e) {
                // ignore parse error
            }
            alert(message);
        },
        complete: function () {
            $("#addNewCourseBtn").prop("disabled", false).text("Add");
        }
    });
});