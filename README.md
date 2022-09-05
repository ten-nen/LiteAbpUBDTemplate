# 基于ABP、Jquery+Boostrap+Tabler、ASPNETCORE MVC三层架构 后台管理的小型Web项目模板，含用户和权限快速开始业务开发
### 后端：[ABP](https://github.com/abpframework/abp) 
### 前端UI库：[tabler](https://github.com/tabler/tabler)

### 环境要求：NET6

## 使用步骤
#### 1、安装模板
``` cmd
dotnet new --install LiteAbpUBDTemplate 
```

#### 2、创建项目，比如Demo，默认sqlserver数据库，-s mysql 切换mysql数据库
``` cmd
dotnet new lat -n Demo            
```

#### 3、安装ef工具
``` cmd
dotnet tool install --global dotnet-ef        
```

#### 4、把本地sqlserver的sa密码修改为123456（Demo\src\Demo.Web\appsettings.json），然后ef生成数据库
``` cmd
cd Demo\src\Demo.DataAccess 
dotnet ef migrations add InitialCreate -o ./Migrations
dotnet ef database update         
```

#### 5、最后net run或者(编译后)iis新建站点指向Demo.Web目录，运行查看
``` cmd
cd Demo\src\Demo.Web
dotnet build
dotnet run --urls http://localhost:5000        
```

## 效果图
#### 登录（账号:admin 密码:123456）
!["登录"](/imgs/login.png "登录")
!["登录"](/imgs/login_m.png "登录")

#### 用户
!["用户"](/imgs/user.png "用户")
!["用户"](/imgs/user_m.png "用户")

#### 角色
!["角色"](/imgs/role_new.png "角色")
!["角色"](/imgs/role_new_m.png "角色")