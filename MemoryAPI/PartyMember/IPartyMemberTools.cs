﻿// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013 Mykezero
//  
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
using MemoryAPI.Navigation;

namespace MemoryAPI
{
    public interface IPartyMemberTools
    {        
        bool UnitPresent { get; }

        int ServerID { get; }

        int Index { get; }

        int TargetIndex { get; }

        string Name { get; }

        int HPCurrent { get; }

        int HPPCurrent { get; }

        int MPCurrent { get; }

        int MPPCurrent { get; }

        int TPCurrent { get; }

        Job Job { get; }

        Job SubJob { get; }

        NpcType NpcType { get; }
    }
}