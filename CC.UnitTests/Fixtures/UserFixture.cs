using CC.API.Models;

namespace CC.API.Fixtures
{
    internal class UserFixture
    {
        public static List<User> GetTestUsers() =>
             [
                new User {
                    Id = 1
                } ];
    }
}
