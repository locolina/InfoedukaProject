using InfoedukaMVC.Models;

namespace InfoedukaMVCTests
{
    public class CourseTests
    {
        [Fact]
        public void CourseName_SetValue_ReturnsCorrectValue()
        {
            // Arrange
            var course = new Course();
            var expectedCourseName = "Test Course";

            // Act
            course.CourseName = expectedCourseName;

            // Assert
            Assert.Equal(expectedCourseName, course.CourseName);

        }
    }
}