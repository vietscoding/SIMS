// Load more courses function
function loadMoreCourses() {
    if (isLoading) {
        console.log("Skipped loadMoreCourses() because already loading.");
        return;
    }
    if (currentPage >= totalPages) {
        console.log("No more pages to load. currentPage:", currentPage, "totalPages:", totalPages);
        return;
    }

    isLoading = true;
    currentPage++;

    console.log("Requesting page", currentPage, "with filters", filters);

    $.ajax({
        url: "/Admin/GetCourses",
        method: "GET",
        data: Object.assign({ page: currentPage }, filters),
        success: function (data) {
            console.log("GetCourses success for page", currentPage, data);
            if (data.courses && data.courses.length > 0) {
                data.courses.forEach(function (c) {
                    sttCounter++;
                    let facultyCell = c.facultyName ? c.facultyName : "<em>Not set yet</em>";
                    let summaryCell = c.courseSummary ? c.courseSummary : "<em>Not set yet</em>";
                    let row = `
                                    <tr>
                                        <td>
                                            <button class="btn btn-info btn-sm view-details" data-id="${c.courseId}">View</button>
                                        </td>
                                        <td>${sttCounter}</td>
                                        <td>${c.courseId}</td>
                                        <td>
                                            <a href="#" class="view-details" data-id="${c.courseId}">
                                                ${c.courseCode || "N/A"}
                                            </a>
                                        </td>
                                        <td>${c.courseName}</td>
                                        <td>${facultyCell}</td>
                                        <td>${c.totalCredits}</td>
                                        <td>${c.lectureCredits}</td>
                                        <td>${c.practicalCredits}</td>
                                        <td>${c.internshipCredits}</td>
                                        <td>${c.capstoneCredits}</td>
                                        <td>${summaryCell}</td>
                                    </tr>
                                `;
                    $("tbody").append(row);
                });

                totalPages = data.totalPages || totalPages;
                console.log("Updated totalPages:", totalPages);
            } else {
                console.log("No more courses in response for page", currentPage);
            }
            isLoading = false;

            // If page still short after appending, try loading next automatically
            autoFillIfNeeded();
        },
        error: function (xhr, status, err) {
            console.error("Error loading more courses", status, err, xhr);
            isLoading = false;
        }
    });
}