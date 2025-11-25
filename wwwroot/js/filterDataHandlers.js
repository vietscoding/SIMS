// Filter handlers for Course.cshtml
$("#applyFilterBtn").on("click", function () {
    const name = $("#filterName").val()?.toString().trim();
    const code = $("#filterCode").val()?.toString().trim();
    const facultyId = $("#filterFacultyId").val();
    const summary = $("#filterSummary").val()?.toString().trim();

    // Build filters object only including non-empty values so model binding of nullable types works
    filters = {};

    if (name) filters.name = name;
    if (code) filters.code = code;
    if (facultyId) {
        if (facultyId !== "") filters.facultyId = facultyId;
    }
    if (summary) filters.summary = summary;

    const lectureMin = $("#filterLectureMin").val();
    const lectureMax = $("#filterLectureMax").val();
    if (lectureMin !== "") filters.lectureMin = lectureMin;
    if (lectureMax !== "") filters.lectureMax = lectureMax;

    const practicalMin = $("#filterPracticalMin").val();
    const practicalMax = $("#filterPracticalMax").val();
    if (practicalMin !== "") filters.practicalMin = practicalMin;
    if (practicalMax !== "") filters.practicalMax = practicalMax;

    const internshipMin = $("#filterInternshipMin").val();
    const internshipMax = $("#filterInternshipMax").val();
    if (internshipMin !== "") filters.internshipMin = internshipMin;
    if (internshipMax !== "") filters.internshipMax = internshipMax;

    const capstoneMin = $("#filterCapstoneMin").val();
    const capstoneMax = $("#filterCapstoneMax").val();
    if (capstoneMin !== "") filters.capstoneMin = capstoneMin;
    if (capstoneMax !== "") filters.capstoneMax = capstoneMax;

    const totalMin = $("#filterTotalMin").val();
    const totalMax = $("#filterTotalMax").val();
    if (totalMin !== "") filters.totalMin = totalMin;
    if (totalMax !== "") filters.totalMax = totalMax;

    // Toggle filter indicator (red dot) if any filter is set
    const anyFilter = Object.values(filters).some(value => value.length > 0);
    if (anyFilter) {
        $("#filterBtn").addClass("has-active-filters");
    } else {
        $("#filterBtn").removeClass("has-active-filters");
    }

    $("#filterModal").hide();

    // Reset and load with new filters
    resetAndLoad();

});