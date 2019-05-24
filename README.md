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
  + Check books
  + Borrow & Return Books
  + Show Self record
  + Change Password
Administrator:
  + Add User
  + Add Books
  + Show All record

```