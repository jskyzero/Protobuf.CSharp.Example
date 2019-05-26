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
  int32, Numbers
BorrowRecord:
  User, user
  Book, book
  Time, BorrowTime
  Time, ReturnTime

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

接下来讲简单概述一下`protocol-buffers`的相关使用方法。

### 