using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Typeyatsu.Core
{
    public interface IGameHubClient
    {
        void OnRivalDisconnected();
        void OnRivalFound();
    }
}
