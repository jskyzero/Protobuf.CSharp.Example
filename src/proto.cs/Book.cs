// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: book.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
/// <summary>Holder for reflection information generated from book.proto</summary>
public static partial class BookReflection {

  #region Descriptor
  /// <summary>File descriptor for book.proto</summary>
  public static pbr::FileDescriptor Descriptor {
    get { return descriptor; }
  }
  private static pbr::FileDescriptor descriptor;

  static BookReflection() {
    byte[] descriptorData = global::System.Convert.FromBase64String(
        string.Concat(
          "Cgpib29rLnByb3RvIigKBEJvb2sSDAoEbmFtZRgBIAEoCRISCgZhdXRob3IY",
          "/////wEgASgJYgZwcm90bzM="));
    descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
        new pbr::FileDescriptor[] { },
        new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
          new pbr::GeneratedClrTypeInfo(typeof(global::Book), global::Book.Parser, new[]{ "Name", "Author" }, null, null, null)
        }));
  }
  #endregion

}
#region Messages
public sealed partial class Book : pb::IMessage<Book> {
  private static readonly pb::MessageParser<Book> _parser = new pb::MessageParser<Book>(() => new Book());
  private pb::UnknownFieldSet _unknownFields;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pb::MessageParser<Book> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::BookReflection.Descriptor.MessageTypes[0]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public Book() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public Book(Book other) : this() {
    name_ = other.name_;
    author_ = other.author_;
    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public Book Clone() {
    return new Book(this);
  }

  /// <summary>Field number for the "name" field.</summary>
  public const int NameFieldNumber = 1;
  private string name_ = "";
  /// <summary>
  /// 1 - 15 is 1 byte
  /// </summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public string Name {
    get { return name_; }
    set {
      name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
    }
  }

  /// <summary>Field number for the "author" field.</summary>
  public const int AuthorFieldNumber = 536870911;
  private string author_ = "";
  /// <summary>
  /// max 2 ^ 29 - 1 is 536870911
  /// </summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public string Author {
    get { return author_; }
    set {
      author_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override bool Equals(object other) {
    return Equals(other as Book);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public bool Equals(Book other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (Name != other.Name) return false;
    if (Author != other.Author) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override int GetHashCode() {
    int hash = 1;
    if (Name.Length != 0) hash ^= Name.GetHashCode();
    if (Author.Length != 0) hash ^= Author.GetHashCode();
    if (_unknownFields != null) {
      hash ^= _unknownFields.GetHashCode();
    }
    return hash;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void WriteTo(pb::CodedOutputStream output) {
    if (Name.Length != 0) {
      output.WriteRawTag(10);
      output.WriteString(Name);
    }
    if (Author.Length != 0) {
      output.WriteRawTag(250, 255, 255, 255, 15);
      output.WriteString(Author);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public int CalculateSize() {
    int size = 0;
    if (Name.Length != 0) {
      size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
    }
    if (Author.Length != 0) {
      size += 5 + pb::CodedOutputStream.ComputeStringSize(Author);
    }
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(Book other) {
    if (other == null) {
      return;
    }
    if (other.Name.Length != 0) {
      Name = other.Name;
    }
    if (other.Author.Length != 0) {
      Author = other.Author;
    }
    _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  public void MergeFrom(pb::CodedInputStream input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
          break;
        case 10: {
          Name = input.ReadString();
          break;
        }
        case 4294967290: {
          Author = input.ReadString();
          break;
        }
      }
    }
  }

}

#endregion


#endregion Designer generated code
