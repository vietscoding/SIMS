namespace SIMS.Models
{
    public class Lecturer
    {
        /// <summary>
        /// Học vị (Academic Degree) là bằng cấp được cấp bởi các cơ sở giáo dục (đại học, cao đẳng) sau khi hoàn thành một chương trình học tập cụ thể. Học vị thể hiện trình độ học vấn của một người.
        /// Cử nhân (Bachelor)
        /// Thạc sĩ (Master)
        /// Tiến sĩ (Doctor/PhD)
        /// </summary>
        public string academicDegree { get ; private set; }

        /// <summary>
        /// Học hàm (Academic Title) là chức danh học thuật, thường được sử dụng trong môi trường giáo dục đại học, thể hiện vai trò hoặc vị trí của một người trong hệ thống giáo dục hoặc nghiên cứu. Học hàm không phải là bằng cấp mà là danh hiệu do cơ quan có thẩm quyền phong tặng, thường dựa trên thành tựu học thuật và nghiên cứu.

        /// Phó Giáo sư (Associate Professor)
        /// Giáo sư (Professor): cao nhất trong lĩnh vực học thuật, thường dành cho những người có thành tựu xuất sắc trong giảng dạy và nghiên cứu.
        /// Trợ lý Giáo sư (Assistant Professor): thường ở giai đoạn đầu sự nghiệp
        /// Giảng viên (Lecturer): Thường là người giảng dạy tại đại học nhưng chưa đạt học hàm Giáo sư
        /// Nhà nghiên cứu (Researcher) : Dùng cho những người tập trung vào nghiên cứu khoa học, có thể không giữ vai trò giảng dạy.
        /// Giáo sư thỉnh giảng (Adjunct Professor): Là giáo sư làm việc bán thời gian hoặc tạm thời tại một trường đại học.
        /// </summary>
        public string academicTitle { get; private set; }


    }
}
