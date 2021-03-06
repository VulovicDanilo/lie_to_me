﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using DataLayer.Models;
using DataLayer.DTOs;
using DataLayer.Repositories;
using WebAPI2.GameStuff;

namespace WebAPI2.Controllers
{
    [RoutePrefix("api/players")]
    public class PlayerController : ApiController
    {
        private UnitOfWork unit = new UnitOfWork();
        [Route("add")]
        [HttpPost]
        public HttpResponseMessage AddPlayer([FromBody] Player player)
        {
            try
            {
                var game = unit.GameRepository.Find(player.GameId);
                var user = unit.UserRepository.Find(player.User_Id);
                player.User = user;
                if (game.Players.Count == 0)
                {
                    game.Owner = player;
                }
                if (game.Players.Count == game.MAX_PLAYERS)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Capacity reached");
                }
                game.Players.Add(player);
                unit.Save();

                GameDictionary.AddPlayer(game);

                // QueueService.BroadcastContext(game.Id.ToString(), MessageQueueChannel.ContextBroadcast, game);
                QueueService.BroadcastLobbyInfo(game.Id.ToString(), "new player has joined the lobby");

                return Request.CreateResponse(HttpStatusCode.OK, player);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }
        }
        [Route("name")]
        [HttpPost]
        public HttpResponseMessage SetName([FromBody] SetNameModel model)
        {
            try
            {                
                if (GameDictionary.SetName(model.gameId, model.playerId, model.fakeName))
                {
                    QueueService.BroadcastLobbyInfo(model.gameId.ToString(), model.fakeName + " has joined...");
                    var game = GameDictionary.Get(model.gameId);
                    QueueService.BroadcastContext(game.Id.ToString(), game);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Conflict);
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }
        }
        [Route("update")]
        [HttpPut]
        public HttpResponseMessage UpdatePlayer([FromBody] Player player)
        {
            try
            {
                if (player.Id != 0)
                {
                    unit.PlayerRepository.Update(player);
                    unit.Save();

                    GameDictionary.UpdatePlayer(player.GameId, player);

                    var game = GameDictionary.Get(player.GameId);

                    QueueService.BroadcastContext(game.Id.ToString(), game);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    throw new Exception("no such player to update");
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }
        }
        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage DeletePlayer([FromUri] int id, [FromUri] int gameId)
        {
            try
            {
                var game = unit.GameRepository.Find(gameId);
                var toDel = game.Players.Where(x => x.Id == id).SingleOrDefault();
                if (toDel != null)
                {
                    game.Players.Remove(toDel);
                }
                unit.Save();

                GameDictionary.RemovePlayer(gameId, id);
                var fullGame = GameDictionary.Get(gameId);


                QueueService.BroadcastContext(game.Id.ToString(), fullGame);
                QueueService.BroadcastLobbyInfo(gameId.ToString(), "player has left the lobby");
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, message);
            }
        }

        [Route("request_role")]
        [HttpPost]
        public HttpResponseMessage RequestRole([FromBody]RoleRequestModel roleRequestModel)
        {
            try
            {
                Game game = GameDictionary.Get(roleRequestModel.gameId);
                var strategy = game.Players.Where(x => x.Id == roleRequestModel.playerId).FirstOrDefault().Role;
                return Request.CreateResponse<RoleStrategy>(HttpStatusCode.OK, strategy);
            } catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [Route("send_message")]
        [HttpPost]
        public HttpResponseMessage SendMessage([FromBody]ChatMessage message)
        {
            var game = GameDictionary.Get(message.GameId);
            var name = game.Players.Where(player => player.Id == message.PlayerId).FirstOrDefault().FakeName;
            var content = name + ": " + message.Content;
            QueueService.BroadcastLobbyInfo(game.Id.ToString(), content);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("voting_vote")]
        [HttpPost]
        public HttpResponseMessage VotingVote([FromBody]VotingVoteModel vote)
        {
            try
            {
                var game = GameDictionary.Get(vote.gameId);
                game.AddVote(vote.voterNumber, vote.votedNumber);

                string Voter = game.Players.Find(x => x.Number == vote.voterNumber).FakeName;
                string Voted = game.Players.Find(x => x.Number == vote.votedNumber).FakeName;

                if (game.GameState == GameState.Voting)
                {
                    QueueService.BroadcastLobbyInfo(game.Id.ToString(), Voter + " voted for " + Voted);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [Route("judgement_vote")]
        [HttpPost]
        public HttpResponseMessage JudgementVote([FromBody]JudgementVoteModel vote)
        {
            try
            {
                var game = GameDictionary.Get(vote.gameId);
                game.AddJudgementVote(vote.voterNumber, vote.vote);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [Route("add_action")]
        [HttpPost]
        public HttpResponseMessage AddAction([FromBody] ExecuteActionModel model)
        {
            try
            {
                var game = GameDictionary.Get(model.gameId);

                game.AddAction(model);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [Route("remove_action")]
        [HttpPost]
        public HttpResponseMessage RemoveAction([FromBody]ExecuteActionModel model)
        {
            try
            {
                var game = GameDictionary.Get(model.gameId);

                game.RemoveAction(model.Who);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [Route("lastwill")]
        [HttpPost]
        public HttpResponseMessage SetLastWill([FromBody]LastWillModel model)
        {
            try
            {
                var game = GameDictionary.Get(model.GameId);
                var player = game.Players.Find(x => x.Number == model.Number);
                player.LastWill = model.LastWill;

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            unit.Dispose();
            base.Dispose(disposing);
        }
    }
}