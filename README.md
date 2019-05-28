# protobuf.csharp.example
`jskyzero` `2019/05/23`

## Structure

```
Data:

User:
  string, Name
  string, PassWord
  enum, RoleType { Default, Administrator }
Book:
  string, Name
  string, Details
  int32, amount
BorrowRecord:
  string, UserName
  string, BookName
  string, BorrowTime
  string, ReturnTime

Commands:

Default:
  + Check Books
  + Borrow & Return Books
  + Show Self Record
  + Change Password
Administrator:
  + Add User
  + Add Books
  + Show All Record
```

## About `protocol-buffers`

接下来讲简单概述一下`protocol-buffers`的相关使用方法，仅仅简单说明一些备忘用的需要注意的点，建议完整阅读参考中的源文档。

+ 使用相关
  1. 下载编译工具
  2. 编写`*.proto`文件
  3. 编译成对应语言的源文件
  4. 工程中添加对应`Google.Protobuf`的库支持
  5. 运行工程
+ 语言相关
  + 简单的例子
  ```
  synatax = "proto3"

  message StructName {
    string str = 1;
    int32 integer = 2;
  }
  ```
  + 一些需要注意的细节
    + 每个字段需要给一个值是为了序列化那边，这个值可以取到[1, 229 - 1]中除去[19000, 19999]的部分。取[1-15]时标志位只使用1byte，否则2byte。
    + 注释支持跨行的`/* 我是注释 */`和单行的`// 我是注释`。
    + 如果某次更新修改了某些字段，建议用`reserved 2 to 10`和`reserved "foo"`的方法保留值和关键词。
  + 相对高级的话题
    + 类型的对应表格

    |.proto|C++|C#|Python|
    |--|--|--|--|
    |double|double|double|float|
    |float|float|float|float|
    |int32|int32|int|int|
    |int64|int64|long|int/long|
    |uint32|uint32|uint|int/long|
    |uint64|uint64|ulong|int/long|
    |sint32|int32|int|int|
    |sint64|int64|long|int/long|
    |fixed32|uint32|uint|int/long|
    |fixed64|uint64|ulong|int/long|
    |sfixed32|int32|int|long|
    |sfixed64|int64|long|int/long|
    |bool|bool|bool|bool|
    |string|string|string|str/unicode|
    |btyes|string|ByteString|str|

    + 对应的整数的选择方法：

    |是否有负数|是否大量大于2^28|应该选择|
    |--|--|--|
    |有|否|int32/int64|
    |有，且多|否|sint32/sint64|
    |有|是|sfixed32/sfixed64|
    |无|否|uint32/uint64|
    |无|是|fixed32/fixed64|

    + 默认值，`空字符串`、`空bytes`、`false`、`0`
    + `Any`、`Oneof`、`Maps` 相关
    + `Package`、 `Compile`、 `Json`、 `Services`相关
+ 风格相关
  + 行长80字符以下，缩进2空格。
  + Package: `my.package`
  + FileName: `lower_snake_case.proto`
  + MessageName: `SongServerRequest`
  + FieldName: `song_name`
  + RepeatedField: `accounts`
  + Enums: `FOO_UNSPECIFIED`
  + Services: `GetSomething`
+ 编码相关
+ 其他（一些数据流与插件相关）

## Reference

+ [Protocol Buffers Language Guide ](https://developers.google.com/protocol-buffers/docs/proto3#updating)