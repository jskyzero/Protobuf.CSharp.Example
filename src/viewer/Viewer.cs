using System;

namespace Protobuf.Csharp.Example.Viewer {
    internal class Viewer {
      private Controller.Controller controller = new Controller.Controller();

      public void Main() {
          controller.SaveDataFile();
      }
    }
}