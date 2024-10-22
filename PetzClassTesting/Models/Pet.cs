using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petzweb.Models
{
  public class Pet
  {
    /// PET INFO ///
    public string Name { get; set; }

    /// STATISTICS ///
    public DateTime DateCreated { get; private set; }
    public int Hunger { get; private set; }
    public int Happiness { get; private set; }
    public int Love { get; private set;}
    public int Health { get; private set; }
    public int Energy { get; private set; }
    public int BodyTemp { get; private set; }
    public bool IsSick { get; private set; }

    /// PET BOUND INVENTORY ///
    public int Coins { get; private set; }

  }
}