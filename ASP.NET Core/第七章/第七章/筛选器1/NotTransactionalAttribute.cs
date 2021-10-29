﻿namespace 筛选器1;
/// <summary>
/// 标注了这个Attribute的Controller方法不自动启动事务
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class NotTransactionalAttribute : Attribute
{
}