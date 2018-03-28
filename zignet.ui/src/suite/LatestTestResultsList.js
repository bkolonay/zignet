import React, { Component } from 'react';
import TestResultRow from './TestResultRow'
import { Link } from 'react-router-dom'

class LatestTestResultsList extends Component {

  render() {
    return (
      <div className="container">
        <h4><Link to='/'>ZigNet</Link></h4>
        <h2 className="text-center">{this.props.suiteName}</h2>
        <table className="table">
          <thead>
            <tr>
              <th scope="col">Name</th>
              <th scope="col">Status</th>
              <th scope="col">History</th>
            </tr>
          </thead>
          <tbody>
            {this.props.testResults.map((testResult) =>
              <TestResultRow key={testResult.TestResultID}
                                testResult={testResult} />
            )}        
          </tbody>
        </table>
      </div>
    );
  }
}

export default LatestTestResultsList;
