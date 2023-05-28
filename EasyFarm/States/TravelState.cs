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
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Xml.Linq;
using EasyFarm.Classes;
using EasyFarm.Context;
using MemoryAPI.Navigation;

namespace EasyFarm.States
{
    public class TravelState : BaseState
    {
        public override bool Check(IGameContext context)
        {
            // Waypoint list is empty.
            if (!context.Config.Route.IsPathSet) return false;

            // Route belongs to a different zone.
            if (context.Config.Route.Zone != context.API.Player.Zone) return false;

            // Has valid target to fight.
            if (context.Target.IsValid) return false;

            // We don't have to rest.
            if (new RestState().Check(context)) return false;

            // We don't have to heal.
            if (new HealingState().Check(context)) return false;

            // We don't need to summon trusts
            if (new SummonTrustsState().Check(context)) return false;

            // We are not bound or struck by an other movement
            // disabling condition.
            if (ProhibitEffects.ProhibitEffectsMovement
                .Intersect(context.API.Player.StatusEffects).Any())
                return false;

            return true;
        }

        public override void Run(IGameContext context)
        {
            context.API.Navigator.DistanceTolerance = 1;

            var currentPosition = context.Config.Route.GetCurrentPosition(context.API.Player.Position);
            if (currentPosition == null || currentPosition.Distance(context.API.Player.Position) <= context.Config.RouteTolerance)
                currentPosition = context.Config.Route.GetNextPosition(context.API.Player.Position);

            double wpDist = currentPosition.Distance(context.API.Player.Position);
            if (wpDist < context.Config.RouteTolerance) {
                context.API.Navigator.Reset();
                context.API.Follow.Reset();
                return;
            } else if (wpDist > context.Config.RouteNavMeshTolerance) {
                var path = context.NavMesh.FindPathBetween(context.API.Player.Position, currentPosition);
                if (path.Count > 0)
                {
                    context.API.Navigator.DistanceTolerance = context.Config.RouteTolerance;

                    while (path.Count > 0 && path.Peek().Distance(context.API.Player.Position) <= context.API.Navigator.DistanceTolerance)
                    {
                        path.Dequeue();
                    }

                    if (path.Count > 0)
                    {
                        Route.NavigateTo(context, path.Peek());
                    }
                    else
                    {
                        context.Config.Route.GetNextPosition(context.API.Player.Position);
                        context.API.Navigator.Reset();
                        context.API.Follow.Reset();
                    }
                    return;
                }
            }
            Route.NavigateTo(context, currentPosition);
        }

        public override void Exit(IGameContext context)
        {
            context.API.Navigator.Reset();
            context.API.Follow.Reset();
        }
    }
}