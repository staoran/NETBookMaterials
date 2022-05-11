using Microsoft.AspNetCore.Builder;
using Zack.EventBus;

namespace CommonInitializer
{
    // 程序中间件扩展
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 封装一些常用的中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseZackDefault(this IApplicationBuilder app)
        {
            // 扫描和使用所有的事件总线
            app.UseEventBus();
            app.UseCors();//启用Cors
            // 处理转发的请求头
            app.UseForwardedHeaders();
            //app.UseHttpsRedirection();//不能与ForwardedHeaders很好的工作，而且webapi项目也没必要配置这个
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}
